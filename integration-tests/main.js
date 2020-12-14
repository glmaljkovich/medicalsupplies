/*
# convert user to admin
\l
\c dbnetcore
\dt
SELECT * FROM "Users";
UPDATE "Users" SET "IsAdmin" = true WHERE "Email" = 'admin@admin.com';


nave use 12
export API_URL=http://medicalsupplies-back.herokuapp.com
export API_URL=http://localhost:8000
export EMAIL=user@user.com
export PASSWORD=user
export ADMIN_EMAIL=admin@medicalsupplies.org
export ADMIN_PASSWORD=123456
node integration-tests/main.js $times $between
*/

var auth = require('./auth');
var suppliesOrders = require('./supplies-orders');

var times = process.argv[2] ? parseInt(process.argv[2]) : 1;
var between = process.argv[3] ? parseInt(process.argv[3]) : 1000;

console.log("times: " + times)
console.log("between: " + between)

var options = {
  apiUrl: process.env.API_URL
}

Promise.all([
  auth.signinOrSignup({
    apiUrl: options.apiUrl,
    email: process.env.EMAIL,
    password: process.env.PASSWORD,
  }),
  auth.signin({
    apiUrl: options.apiUrl,
    email: process.env.ADMIN_EMAIL,
    password: process.env.ADMIN_PASSWORD,
  })
])
//.then((token)=> console.log("token",token) & token)
.then(([userToken, adminToken])=> {
  Promise.all([auth.profile(userToken, options)
  .then((userProfile)=>{
    console.log('userProfile', userProfile)
  }),
  auth.profile(adminToken, options)
  .then((adminProfile)=>{
    console.log('adminProfile', adminProfile)
  })])
  .then(()=>{
    var result = {
      create:{
        byStatus:{},
        maxTime: 0,
        minTime: Infinity,
        averageTime: null,
        totalTime: 0,
        amount: 0
      },
      delete:{
        byStatus:{},
        maxTime: 0,
        minTime: Infinity,
        averageTime: null,
        totalTime: 0,
        amount: 0
      }
    }
    var doProcess = function(target, time, status){
      target.amount ++;
      if(typeof target.byStatus[status] == 'undefined'){
        target.byStatus[status] = 0
      }
      target.byStatus[status] ++
      if(target.maxTime < time){
        target.maxTime = time
      }
      if(target.minTime > time){
        target.minTime = time
      }
      target.totalTime += time
    }
    return repeat((index, total)=>{
      if(index == total){
        console.log("last operation")
      }
      var init = time()
      return suppliesOrders.create({
        apiUrl: options.apiUrl,
        token: userToken,
        supplyType: 'MASCARA_PROTECTORA',
        areaId: 'ATENCION_A_PACIENTES'
      })
      .catch((reason)=>{
        console.error("An error occurs on order supply creation", reason, init, end)
        var end = time()
        doProcess(result.create, end - init, '!200')
        throw reason
      })
      .then((id)=>{
        var end = time()
        console.log('created suppliesOrder id', id, init, end)
        doProcess(result.create, end - init, '200')
        var init2 = time()
        return suppliesOrders.delete({
          apiUrl: options.apiUrl,
          token: userToken,
          id: id
        })
        .catch((reason)=>{
          var end = time()
          console.error("An error occurs on order supply deletion", reason, init2, end)
          doProcess(result.delete, end - init2, '!200')
          throw reason
        })
        .then((id)=>{
          var end = time()
          console.log('deleted suppliesOrder id', id, init2, end)
          doProcess(result.delete, end - init2, '200')
        })
      })
    }, times, between)
    .then(()=>result)
  })
  .then((result)=>{
    console.log('task finished')
    if(result.create.amount > 0){
      result.create.averageTime = result.create.totalTime / result.create.amount
    }
    if(result.delete.amount > 0){
      result.delete.averageTime = result.delete.totalTime / result.delete.amount
    }
    console.log(result)
  })
})


function repeat(f, times, between){
  var iterations = 0;
  var done = 0;
  return new Promise((resolve, reject)=>{
    var interval = setInterval(function(){
      iterations++
      if(iterations >= times){
        clearInterval(interval)
      }
      f(iterations, times).finally(()=>{
        done++;
        if(done >=times){
          resolve();
        }
      });
    }, between);
  })
  // return function(){
  //   clearInterval(interval)
  // }
}


function time(){
  var d = new Date();
  return d.getTime();
}