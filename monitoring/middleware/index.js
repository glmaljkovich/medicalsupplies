var https = require("https");
var http = require("http");
var riemann = require('riemannjs').createClient({
  host: process.env.RIEMANN_HOST,
  port: process.env.RIEMANN_PORT? parseInt(process.env.RIEMANN_PORT) : 5555
});

const PORT = process.env.PORT || '8000'
const TARGET_URL = process.env.TARGET_URL

if(!TARGET_URL){
 throw {
   message: "TARGET_URL environment variable expected"
 }
}

var notify = function(initTime, statusCode){
  var delta = process.hrtime(initTime);
  var deltaSeconds = delta[0] + (delta[1] / 1000000)
  console.log("request end in: " + deltaSeconds + "s" )
  riemann.send(riemann.Event({
    service: 'http-response-time',
    metric:  deltaSeconds,
    tags:    ['nonblocking'],
    attributes: [{
      key:'application',
      value: process.env.APPLICATION_NAME || 'UNKNOW'
    }],
    state:   'ok'
  }));
  riemann.send(riemann.Event({
    service: 'http-status-code',
    metric:  statusCode || 0,
    tags:    ['nonblocking'],
    attributes: [{
      key:'application',
      value: process.env.APPLICATION_NAME || 'UNKNOW'
    }],
    state:   'ok'
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

function logerror(error) {
  console.log("Got error: " + error.message);
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
        console.log("[status][ " + (index++) + " ]")
        console.log("connections count: " + count)
      }
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
      procesoID = setInterval(status, timeInMillis || 5000);
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
    console.log("listening on port: " + PORT);
    console.log("endpoint: ", endpoint);
    statusLoop.start(server)
  }
}



var server = http.createServer(handler);
server.listen(PORT, onListenStart(server));

server.on('close', function onServerClose(){
  statusLoop.stop()
})



