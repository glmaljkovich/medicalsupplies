/*
nave use 12
export API_URL=http://localhost:5000
export EMAIL=myemail
export PASSWORD=mypassword
node integration-tests/main.js
*/

var auth = require('./auth');
var suppliesOrders = require('./supplies-orders');

var options = {
  apiUrl: process.env.API_URL
}
 
Promise.all({
  userToken: auth.signinOrSignup({
    apiUrl: options.apiUrl,
    email: process.env.EMAIL,
    password: process.env.PASSWORD,
  }),
  adminToken: auth.signin({
    apiUrl: options.apiUrl,
    email: process.env.ADMIN_EMAIL,
    password: process.env.ADMIN_PASSWORD,
  })
})
//.then((token)=> console.log("token",token) & token)
.then(({userToken, adminToken})=> {
  auth.profile(userToken, options)
  .then((userProfile)=>{
    console.log('userProfile', userProfile)
  })
  auth.profile(adminToken, options)
  .then((adminProfile)=>{
    console.log('adminProfile', adminProfile)
  })
  suppliesOrders.create({
    apiUrl: options.apiUrl,
    token: userToken,
    suppliesType: '',
    areaId: ''
  }).then((id)=>{
    console.log('new suppliesOrder id', id)
  })
})
