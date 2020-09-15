var request = require('./request');

module.exports = function(options){
  return request({
    uri:options.apiUrl + "/user/signin",
    data:{
      email: options.email,
      password: options.password,
    }
  })
  .then((res)=>{
    return JSON.parse(res.data).access_token
  })
}