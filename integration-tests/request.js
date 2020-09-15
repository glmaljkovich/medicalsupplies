var http = require('http');
var https = require('https');

var serialize = function(query){
    var result = "";
    var append = false;
    for(var attrName in query){
        if(query.hasOwnProperty(attrName)){
            result += (append?"&":"") + attrName + "=" + query[attrName]
            append = true;
        }
    }
    return result;
}

module.exports = function(options){
  if(typeof options !== 'object'){
    throw new TypeError("reqeust options required")
  }
  var agent = /^https/.test(options.uri) ? https:http
  var uri = options.uri
  uri += (typeof options.query == 'object') ? (/^\?/.test(uri)? "&" : "?") + serialize(options.query) : '';
  var data = typeof options.data == 'object' ? JSON.stringify(options.data) : options.data
  return new Promise(function(resolve, reject){
    var request = agent.request(uri, {
      method: options.method,
      headers: options.headers
    }, function(res){
        var buffer = ""
        res.setEncoding('utf8');
        res.on('data', (chunk) => {
          buffer += chunk
        });
        res.on('end', () => {
          resolve({
            data:buffer,
            headers: res.headers,
            statusCode: res.statusCode
          })
        });
    })
    request.on('error', (e) => {
      reject(e)
    });
    data && request.write(data);
    request.end();
  })
}