const apiUrl = window['api_url'];
import User from './User';

function tryParseJson(raw){
    try {
        return JSON.parse(raw);
    } catch (error) {
        return raw;
    }
}

var messages = [];

export default class Message {
    constructor(text, username, id, time) {
        this.Username = username;
        this.Text = text;
        this.Id = id;
        this.Time = time;
    }

    static getAll(cb){
        if (typeof cb !== "function") return messages;
        var xhr = new XMLHttpRequest();
        xhr.open('GET', apiUrl+'/messages');
        xhr.setRequestHeader("Authorization", User.getToken());
        xhr.onload = () => { 
            const res = tryParseJson(xhr.response);
            if (xhr.status === 200){
                messages = res.map((m) => { return new Message(m.Text, m.Username, m.Id, m.Time)});
                cb(messages, xhr.status, res);
            } else {
                cb(null, xhr.status, res);
            }
        };
        xhr.send();
    }

    send(cb){
        var xhr = new XMLHttpRequest();
        xhr.open('POST', apiUrl+'/send');
        xhr.setRequestHeader("Content-type", "application/json");
        xhr.setRequestHeader("Authorization", User.getToken());
        xhr.onload = () => { 
            const res = tryParseJson(xhr.response);
            if (xhr.status === 200){
                var newMessage = new Message(res.Text, res.Username, res.Id, res.Time);
                this.Id = newMessage.Id;
                this.Text = newMessage.Text;
                this.Username = newMessage.Username;
                this.Time = newMessage.Time;
                cb(newMessage, xhr.status, res);
            } else {
                cb(null, xhr.status, res.Message);
            }
        };
        xhr.send(JSON.stringify({ Text: this.Text }));
    }
}