var users = null;
var me = null;
const apiUrl = window['api_url'];

function tryParseJson(raw){
    try {
        return JSON.parse(raw);
    } catch (error) {
        return raw;
    }
}

export default class User {
    constructor(username, name, admin) {
        this.Username = username;
        this.DisplayName = name;
        this.Admin = admin;
    }

    static getAll(cb){
        if (typeof cb !== "function") 
            return users !== null ? users : {};
        var xhr = new XMLHttpRequest();
        xhr.open('GET', apiUrl+'/users');
        xhr.setRequestHeader("Authorization", localStorage.token);
        xhr.onload = () => { 
            const res = tryParseJson(xhr.response);
            if (xhr.status === 200){
                users ={};
                res.forEach((u) => {
                    users[u.Username] = new User(u.Username, u.DisplayName, u.Admin);
                });
                cb(users, 200, {});
            } else {
                cb(null, xhr.status, res);
            }
        };
        xhr.send();
    }

    static getMe(cb){
        if (typeof cb !== "function") 
            return me !== null 
                ? me 
                : { DisplayName: "...", Admin: true };
                
        if (me !== null)
            cb(me, 200, {});
        var xhr = new XMLHttpRequest();
        xhr.open('GET', apiUrl+'/login');
        xhr.setRequestHeader("Authorization", localStorage.token);
        xhr.onload = () => { 
            me = null;
            const res = tryParseJson(xhr.response);
            if (xhr.status === 200 && res.Success){
                me = new User(res.Account.Username, res.Account.DisplayName, res.Account.Admin);
                cb(me, 200, {});
            } else {
                cb(me, xhr.status, res);
            }
        };
        xhr.send();
    }

    static get(username, cb){
        if (typeof cb !== "function") 
            return users !== null && users.hasOwnProperty(username) 
                ? users[username] 
                : { DisplayName: "..." };

        if (users === null) return { DisplayName: "..." };
        if (users.hasOwnProperty(username))
            cb(users[username], 200, {});
        var xhr = new XMLHttpRequest();
        xhr.open('GET', apiUrl+'/users/'+username);
        xhr.setRequestHeader("Authorization", localStorage.token);
        xhr.onload = () => { 
            const res = tryParseJson(xhr.response);
            let user = null;
            if (xhr.status === 200){
                user = new User(res.Username, res.DisplayName, res.Admin);
                users[user.Username] = user;
            }
            cb(user, xhr.status, res);
        };
        xhr.send();
    }

    static login(username, password, cb){
        var xhr = new XMLHttpRequest();
        xhr.open('POST', apiUrl+'/login');
        xhr.setRequestHeader("Content-type", "application/json");
        xhr.onload = () => { 
            var res = tryParseJson(xhr.response);
            me = null;
            if (xhr.status === 200 && res.Success){
                localStorage.token = 'Basic ' + new Buffer(username + ':' + password).toString('base64');
                me = new User(res.Account.Username, res.Account.DisplayName, res.Account.Admin)
            }
            cb(me, xhr.status, res);
        };
        xhr.send(JSON.stringify({ Username: username, Password: password }));
    }

    static logout (cb) {
        delete localStorage.token;
        me = null;
        if (typeof cb === "function") cb();
    }

    static getToken () {
        return localStorage.token;
    }

    static loggedIn () {
        return !!localStorage.token;
    }

    update(cb){
        var xhr = new XMLHttpRequest();
        xhr.open('PUT', apiUrl+'/users/'+this.Username);
        xhr.setRequestHeader("Authorization", localStorage.token);
        xhr.setRequestHeader("Content-type", "application/json");
        xhr.onload = () => { 
            const res = tryParseJson(xhr.response);
            if (xhr.status === 200)
                users[this.Username] = new User(res.Username, res.DisplayName, res.Admin);
            cb(users[this.Username], xhr.status, res);
        };
        xhr.send(JSON.stringify({ Username: this.Username, DisplayName: this.DisplayName, Admin: this.Admin }));
    }

    delete(cb){
        var xhr = new XMLHttpRequest();
        xhr.open('DELETE', apiUrl+'/users/'+this.Username);
        xhr.setRequestHeader("Authorization", localStorage.token);
        xhr.onload = () => { 
            const res = tryParseJson(xhr.response);
            if (xhr.status === 200)
                delete users[this.Username];
            cb(null, xhr.status, res);
        };
        xhr.send();
    }

    register(password, cb){
        var xhr = new XMLHttpRequest();
        xhr.open('POST', apiUrl+'/register');
        xhr.setRequestHeader("Content-type", "application/json");
        xhr.onload = () => { 
            const res = tryParseJson(xhr.response);
            if (xhr.status === 200){
                cb(new User(res.Username, res.DisplayName, res.Admin), xhr.status, res);
            } else {
                cb(null, xhr.status, res.Message);
            }
        };
        xhr.send(JSON.stringify({ Username: this.Username, Password: password, DisplayName : this.DisplayName }));
    }
}