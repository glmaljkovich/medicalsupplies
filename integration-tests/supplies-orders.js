var request = require('./request');

module.exports = {
  get: function(options){
    var pathSufix = typeof options.id == 'string' ? '/'+options.id: ''
    return request({
      uri:options.apiUrl + "/supplies-orders" + pathSufix,
      method: 'GET',
      headers:{
        'content-type':'application/json',
        'Authorization': 'Bearer ' + options.token
      }
    })
    .then((res)=>{
      return JSON.parse(res.data)
    })
  },
  create: function(options){
    return request({
      uri:options.apiUrl + "/supplies-orders",
      method: 'POST',
      headers:{
        'content-type':'application/json',
        'Authorization': 'Bearer ' + options.token
      },
      data:{
        supply_type: options.supplyType,
        supply_attributes: options.supplyAttributes,
        area_id: options.areaId,
      }
    })
    .then((res)=>{
      return JSON.parse(res.data).id
    })
  },
  delete: function(options){
    return request({
      uri:options.apiUrl + "/supplies-orders/" + options.id,
      method: 'DELETE',
      headers:{
        'content-type':'application/json',
        'Authorization': 'Bearer ' + options.token
      }
    })
  }
}