import { apiUrl } from '../config';
import { getToken } from '../auth';

function tryParseJson(raw){
    try {
        return JSON.parse(raw);
    } catch (error) {
        return raw;
    }
}

function getAllUsers(cb){
    var xhr = new XMLHttpRequest();
    xhr.open('GET', apiUrl+'/users');
    xhr.setRequestHeader("Authorization", getToken());
    xhr.onload = () => { cb(xhr.status, tryParseJson(xhr.response)); };
    xhr.send();
}

function getUserByUsername(username, cb){
    var xhr = new XMLHttpRequest();
    xhr.open('GET', apiUrl+'/users/'+username);
    xhr.setRequestHeader("Authorization", getToken());
    xhr.onload = () => { cb(xhr.status, tryParseJson(xhr.response)); };
    xhr.send();
}

function registerNewUser(username, password, name, cb){
    var xhr = new XMLHttpRequest();
    xhr.open('POST', apiUrl+'/register');
    xhr.setRequestHeader("Content-type", "application/json");
    xhr.onload = () => { cb(xhr.status, tryParseJson(xhr.response)); };
    xhr.send(JSON.stringify({ Username: username, Password: password, DisplayName : name }));
}

function editUser(username, name, admin, cb){
    var xhr = new XMLHttpRequest();
    xhr.open('PUT', apiUrl+'/users/'+username);
    xhr.setRequestHeader("Authorization", getToken());
    xhr.setRequestHeader("Content-type", "application/json");
    xhr.onload = () => { cb(xhr.status, tryParseJson(xhr.response)); };
    xhr.send(JSON.stringify({ Username: username, DisplayName: name, Admin: admin }));
}

function deleteUser(username, cb){
    var xhr = new XMLHttpRequest();
    xhr.open('DELETE', apiUrl+'/users/'+username);
    xhr.setRequestHeader("Authorization", getToken());
    xhr.onload = () => { cb(xhr.status, tryParseJson(xhr.response)); };
    xhr.send();
}

export default class User {
    constructor(username, name, admin) {
        this.Username = username;
        this.DisplayName = name;
        this.Admin = admin;
    }

    static getAll(cb){
        getAllUsers((status, res) => {
            if (status === 200){
                cb(res.map((u) => { return new User(u.Username, u.DisplayName, u.Admin)}), status, res);
            } else {
                cb(null, status, res);
            }
        });
    }

    static get(username, cb){
        getUserByUsername(username, (status, res) => {
            if (status === 200){
                cb(new User(res.Username, res.DisplayName, res.Admin), status, res);
            } else {
                cb(null, status, res);
            }
        });
    }

    edit(cb){
        editUser(this.Username, this.DisplayName, this.Admin, (status, res) => {
            cb(new User(res.Username, res.DisplayName, res.Admin), status, res);
        });
    }

    delete(cb){
        deleteUser(this.Username, (status, res) => {
            cb(null, status, res);
        });
    }

    register(password, cb){
        registerNewUser(this.Username, password, this.DisplayName, (status, res) => {
            if (status === 200){
                cb(new User(res.Username, res.DisplayName, res.Admin), status, res);
            } else {
                cb(null, status, res.Message);
            }
        });
    }
}