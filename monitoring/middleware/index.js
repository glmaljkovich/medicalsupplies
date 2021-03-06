var https = require("https");
var http = require("http");
var hostname = require('os').hostname();

var log = function(){
  if(process.env.MIDDLEWARE_LOGS){
    console.log.apply(console, arguments)
  }
}

var riemann = require('riemannjs').createClient({
  host: process.env.RIEMANN_HOST,
  port: process.env.RIEMANN_PORT? parseInt(process.env.RIEMANN_PORT) : 5555
});

var riemannAttributes = (function(){
  var result = [{
    key:'hostname',
    value: hostname
  }]
  if(typeof process.env.RIEMANN_ATTRIBUTES != 'string'){
    return result;
  }
  var input = process.env.RIEMANN_ATTRIBUTES
  var valuesRegex = new RegExp('([^=]+)=([^,]+),?', 'g');
  var matches;
  while (matches = valuesRegex.exec(input)) {
    result.push({
      key: matches[1],
      value: matches[2]
    })
  }
  return result;
})()


const PORT = process.env.PORT || '8000'
const TARGET_URL = process.env.TARGET_URL
const HTTP_RESPONSE_THRESHOLD = process.env.HTTP_RESPONSE_THRESHOLD? parseFloat(process.env.HTTP_RESPONSE_THRESHOLD) : 30.0;

log('RIEMANN_HOST', process.env.RIEMANN_HOST)
log('RIEMANN_PORT', process.env.RIEMANN_PORT)
log('HTTP_RESPONSE_THRESHOLD', HTTP_RESPONSE_THRESHOLD)
log('RIEMANN_ATTRIBUTES', JSON.stringify(riemannAttributes, null, 2))
log('TARGET_URL', process.env.TARGET_URL)
log('PORT', process.env.PORT)

if(!TARGET_URL){
  throw {
    message: "TARGET_URL environment variable expected"
  }
}

var notify = function(initTime, statusCode){
  var delta = process.hrtime(initTime);
  var deltaSeconds = delta[0] + (delta[1] / 1000000000)
  var deltaMilliSeconds = delta[0] * 1000 + (delta[1] / 1000000)
  log("request end in: " + deltaSeconds + "s" )
  var responseTime = deltaSeconds > HTTP_RESPONSE_THRESHOLD? 1 : deltaSeconds / HTTP_RESPONSE_THRESHOLD
  riemann.send(riemann.Event({
    service: 'http-response-time-normalized',
    metric: responseTime,
    tags: ['normalized'],
    host: process.env.RIEMANN_APPLICATION_NAME || 'UNKNOW',
    attributes: riemannAttributes,
    state: deltaSeconds > HTTP_RESPONSE_THRESHOLD? 'error':'ok'
  }));
  riemann.send(riemann.Event({
    service: 'http-response-time',
    metric: deltaMilliSeconds,
    tags: ['raw'],
    host: process.env.RIEMANN_APPLICATION_NAME || 'UNKNOW',
    attributes: riemannAttributes,
    state: deltaSeconds > HTTP_RESPONSE_THRESHOLD? 'error':'ok'
  }));
  riemann.send(riemann.Event({
    service: 'http-status-code-normalized',
    metric: statusCode ? statusCode / 1000.0 : 0,
    tags: ['normalized'],
    host: process.env.RIEMANN_APPLICATION_NAME || 'UNKNOW',
    attributes: riemannAttributes,
    state: !statusCode || statusCode >= 500  ? 'error' : (statusCode < 300 ? 'ok' : 'warn')
  }));
  riemann.send(riemann.Event({
    service: 'http-status-code',
    metric: statusCode,
    tags: ['raw'],
    host: process.env.RIEMANN_APPLICATION_NAME || 'UNKNOW',
    attributes: riemannAttributes,
    state: !statusCode || statusCode >= 500  ? 'error' : (statusCode < 300 ? 'ok' : 'warn')
  }));
}

var endpoint = (function(url){
  var match = /^(((https?)(?:\:))?\/\/)?([^\/\:]+)(:(\d+))?/.exec(url);
  return {
    protocol: match[3] == 'https' ? 'https' : 'http',
    host: match[4],
    port: match[6] && match[6].length > 0 ? parseInt(match[6]) : undefined
  }
})(TARGET_URL);

var agent = endpoint.protocol == 'http' ? http : https;

const giveBack = function giveBack(target, client, initTime) {
  var headersCopy = Object.assign({}, target.response.headers)
  client.response.writeHead(target.response.statusCode, headersCopy);
  if (typeof target.response.headers['content-length'] !== 'undefined') {
    client.response.chunkedEncoding = false;
  }
  target.response.on('data', function (chunk) {
    target.partials.push(chunk)
    client.response.write(chunk);
  });
  target.response.on('end', function () {
    target.body = Buffer.concat(target.partials);
    client.response.end();
    notify(initTime, target.response.statusCode);
  });
}

const forward = function forward(client) {
  var initTime = process.hrtime()
  var clientHeaders = client.request.headers;
  var headers = Object.assign({}, clientHeaders)
  var contentLength = clientHeaders['content-length'];
  var chunkedData = typeof contentLength == 'undefined';
  if (!chunkedData) {
    headers['content-length'] = clientHeaders['content-length']
  }
  headers.host = endpoint.host + (endpoint.port ? ':' + endpoint.port : '');
  var options = {
    method: client.request.method,
    hostname: endpoint.host,
    path: client.request.url,
    headers: headers,
    port: endpoint.port
  };
  var target = {
    request: agent.request(options, function(response){
      target.response = response;
      giveBack(target, client, initTime);
    }),
    partials: []
  }
  target.request.on('error', function onTargetError(reason){
    console.error('target request error', reason)
    target.error = reason || true
    client.request.end();
  });
  client.request.on('error', function onClientError(reason){
    console.error('client request error', reason)
    client.error = reason || true
    target.request.end();
    notify(initTime, 0);
  });
  client.request.on('data', function onClientData(chunk){
    client.partials.push(chunk)
    target.request.write(chunk);
  });
  client.request.on('end', function onClientEnd(){
    //client.body = Buffer.concat(client.partials);
    target.request.end();
  });
}

const handler = function handler(request, response){
  return forward({
    request:request, 
    response:response,
    partials: []
  });
} 

const statusLoop = (function startStatusLoop(){
  var server;
  var index;
  const status = function status(){
    server.getConnections(function(error, count){
      if(count > 0){
        log("[status][ " + (index++) + " ]")
        log("connections count: " + count)
      }
      riemann.send(riemann.Event({
        service: 'http-connections-normalized',
        metric: count > 100? 1 : count / 100,
        tags: ['normalized'],
        host: process.env.RIEMANN_APPLICATION_NAME || 'UNKNOW',
        attributes: riemannAttributes,
        state: count > 100? 'warn' : 'ok',
      }));
      riemann.send(riemann.Event({
        service: 'http-connections',
        metric: count,
        tags: ['raw'],
        host: process.env.RIEMANN_APPLICATION_NAME || 'UNKNOW',
        attributes: riemannAttributes,
        state: count > 100? 'warn' : 'ok',
      }));
    })
  }
  var procesoID;
  return {
    start:function(_server, timeInMillis){
      server = _server;
      index = 0;
      if(procesoID){
        clearInterval(procesoID);
      }
      procesoID = setInterval(status, timeInMillis || 1000);
    },
    stop: function(){
      if(procesoID){
        clearInterval(procesoID);
      }
    }
  }
})()

const onListenStart = function onListenStart(server){
  return function onListenStartWithServer(){
    log("listening on port: " + PORT);
    log("endpoint: ", endpoint);
    statusLoop.start(server)
  }
}

var server = http.createServer(handler);
server.listen(PORT, onListenStart(server));

server.on('close', function onServerClose(){
  statusLoop.stop()
})



