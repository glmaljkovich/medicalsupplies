var request = require('./request');

module.exports = {
  signup: function(options){
    return request({
      uri:options.apiUrl + "/user",
      method: 'POST',
      headers:{
        'content-type':'application/json'
      },
      data:{
        email: options.email,
        password: options.password,
      }
    })
  },
  signin: function(options){
    return request({
      uri:options.apiUrl + "/user/signin",
      method: 'POST',
      headers:{
        'content-type':'application/json'
      },
      data:{
        email: options.email,
        password: options.password,
      }
    })
    .then((res)=>{
      return JSON.parse(res.data).access_token
    })
  },
  signinOrSignup(options){
    return this.signin(options)
    .catch((reason)=>{
      console.warn('signin failed with: ', reason, 'trying signup')
      return this.signup(options)
      .catch((reason)=>{
        console.warn('signup failed with: ', reason, 'aborting')
        throw reason
      })
      .then(()=> this.signin(options))
    })
  },
  profile: function(token, options){
    return request({
      uri:options.apiUrl + "/user/profile",
      method: 'GET',
      headers:{
        'content-type':'application/json',
        'Authorization': 'Bearer ' + token
      }
    })
    .then((res)=>{
      return JSON.parse(res.data)
    })
  }
}