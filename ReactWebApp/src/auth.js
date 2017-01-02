/******* LOGIN *******

Example POST body
{
  "Username" : "admin",
  "Password" : "Geslo.01"
}

Example response #1
{
  "Success" : true,
  "Admin"   : true,
  "Error"   : null,
  "Account" : {
    "DisplayName" : "Admin Adminus",
    "Username"    : "admin",
    "Admin"       : true
  }
}

Example response #2
{
  "Success" : false,
  "Admin"   : false,
  "Error"   : "UserNotFound",
  "Account" : null
}

Possible error values:
[
  null,
  "UserNotFound",
  "InvalidPassword",
  "InvalidCredentials" // only at GET request with auth token
]

 */

import { apiUrl } from './config';

export function login (username, password, cb) {
  var xhr = new XMLHttpRequest();
  xhr.open('POST', apiUrl+'/login');
  xhr.setRequestHeader("Content-type", "application/json");
  xhr.onload = () => { 
    var res = JSON.parse(xhr.response);
    if (xhr.status === 200 && res.Success){
      localStorage.token = 'Basic ' + new Buffer(username + ':' + password).toString('base64');
      localStorage.user = JSON.stringify(res.Account);
    }
    cb(xhr.status, res);
  };
  xhr.send(JSON.stringify({ Username: username, Password: password }));
}

export function refreshUser (cb) {
    var xhr = new XMLHttpRequest();
    xhr.open('GET', apiUrl+'/login');
    xhr.setRequestHeader("Authorization", localStorage.token);
    xhr.onload = () => { 
      var res = JSON.parse(xhr.response);
      if (xhr.status === 200 && res.Success){
        localStorage.user = JSON.stringify(res.Account);
      }
      cb(xhr.status, res);
    };
    xhr.send();
  }

export function logout (cb) {
  delete localStorage.token;
  delete localStorage.user;
  if (typeof cb === "function") cb();
}

export function getToken () {
  return localStorage.token;
}

export function getUser () {
  return JSON.parse(localStorage.user);
}

export function loggedIn () {
  return !!localStorage.token;
}