import { apiUrl } from './config';

module.exports = {

  login: (username, password, cb) => {
    var xhr = new XMLHttpRequest();
    xhr.open('POST', apiUrl+'/login');
    xhr.setRequestHeader("Content-type", "application/json");
    xhr.onload = () => { 
      var res = JSON.parse(xhr.response);
      if (xhr.status === 200 && res.Success){
        localStorage.token = 'Basic ' + new Buffer(username + ':' + password).toString('base64');
      }
      cb(xhr.status, res);
    };
    xhr.send(JSON.stringify({ Username: username, Password: password }));
  },

  logout: (cb) => {
    delete localStorage.token;
    if (typeof cb === "function") cb();
  },

  getToken: () => {
    return localStorage.token
  },

  loggedIn: () => {
    return !!localStorage.token
  }

}