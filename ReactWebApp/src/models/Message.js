import { apiUrl } from '../config';
import { getToken } from '../auth';

function tryParseJson(raw){
    try {
        return JSON.parse(raw);
    } catch (error) {
        return raw;
    }
}

function getAllMessages(cb){
    var xhr = new XMLHttpRequest();
    xhr.open('GET', apiUrl+'/messages');
    xhr.setRequestHeader("Authorization", getToken());
    xhr.onload = () => { cb(xhr.status, tryParseJson(xhr.response)); };
    xhr.send();
}

function getMessagesSince(id, cb){
    var xhr = new XMLHttpRequest();
    xhr.open('GET', apiUrl+'/messages/'+id);
    xhr.setRequestHeader("Authorization", getToken());
    xhr.onload = () => { cb(xhr.status, tryParseJson(xhr.response)); };
    xhr.send();
}

function sendMessage(text, cb){
    var xhr = new XMLHttpRequest();
    xhr.open('POST', apiUrl+'/send');
    xhr.setRequestHeader("Content-type", "application/json");
    xhr.setRequestHeader("Authorization", getToken());
    xhr.onload = () => { cb(xhr.status, tryParseJson(xhr.response)); };
    xhr.send(JSON.stringify({ Text: text }));
}

export default class Message {
    constructor(text, username, id, time) {
        this.Username = username;
        this.Text = text;
        this.Id = id;
        this.Time = time;
    }

    static getAll(cb){
        getAllMessages((status, res) => {
            if (status === 200){
                cb(res.map((m) => { return new Message(m.Text, m.Username, m.Id, m.Time)}), status, res);
            } else {
                cb(null, status, res);
            }
        });
    }

    static getSince(id, cb){
        getMessagesSince(id, (status, res) => {
            if (status === 200){
                cb(res.map((m) => { return new Message(m.Text, m.Username, m.Id, m.Time)}), status, res);
            } else {
                cb(null, status, res);
            }
        });
    }

    send(cb){
        sendMessage(this.Text, (status, res) => {
            if (status === 200){
                var newMessage = new Message(res.Text, res.Username, res.Id, res.Time);
                this.Id = newMessage.Id;
                this.Text = newMessage.Text;
                this.Username = newMessage.Username;
                this.Time = newMessage.Time;
                cb(newMessage, status, res);
            } else {
                cb(null, status, res.Message);
            }
        });
    }
}