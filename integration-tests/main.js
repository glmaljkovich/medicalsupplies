/*
nave use 12
export API_URL=http://localhost:5000
export EMAIL=myemail
export PASSWORD=mypassword
node integration-tests/main.js
*/

var auth = require('./auth');

var options = {
  apiUrl: process.env.API_URL,
  email: process.env.EMAIL,
  password: process.env.PASSWORD
} 

var token = auth(options)