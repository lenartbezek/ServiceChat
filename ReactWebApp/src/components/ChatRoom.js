import React from 'react';
import Overlay from './Overlay';
import UserList from './UserList';
import MessageList from './MessageList';
import FlatButton from 'material-ui/FlatButton';
import TextField from 'material-ui/TextField';
import Paper from 'material-ui/Paper';
import ContentSend from 'material-ui/svg-icons/content/send';
import { browserHistory } from 'react-router';

import User from '../models/User';
import Message from '../models/Message';
import * as auth from '../auth';

const userListStyle = {
    width: "16rem",
    margin: "0.5rem"
}

const messageListStyle = {
    flex: 1,
    margin: "0.5rem"
}

const messageContainerStyle = {
    padding: "1rem",
    margin: "0.5rem",
    display: "flex",
    flexDirection: "row"
}

const containerStyle = {
    height: "calc(100vh - 7rem - 64px)",
    display: "flex",
    flexDirection: "row"
}

class ChatRoom extends React.Component {
    text = "";
    getUserDataInterval = null;
    getNewMessagesInterval = null;

    state = {
        messages: [],
        users: null,
        me: auth.getUser(),
        authError: false,
        connectionError: false
    };

    componentDidMount = () => {
        this.getUserData();
        this.getNewMessages();
        this.getUserDataInterval = window.setInterval(this.getUserData, 15000);
        this.getNewMessagesInterval = window.setInterval(this.getNewMessages, 5000);

        var rc = this;
        document.getElementById("messageTextField")
            .addEventListener("keyup", function(event) {
            if (event.keyCode === 13) {
                rc.handleSendClick();
            }
        });
    };

    componentWillUnmount = () => {
        if (this.getUserDataInterval !== null)
            window.clearInterval(this.getUserDataInterval);
        if (this.getNewMessagesInterval !== null)
            window.clearInterval(this.getNewMessagesInterval);
    }

    getUserData = () => {
        auth.refreshUser((status, res) => {
            if (status === 200 && res.Status){
                this.setState({ me: auth.getUser() });
            }
        });
        User.getAll((userArray, status, res) => {
            if (status === 200) {
                let userDict = null;
                if (typeof userArray !== 'undefined'){
                    userDict = {};
                    userArray.forEach((user) => {
                        userDict[user.Username] = user;
                    });
                }
                this.setState({ users: userDict });
            } else if (status === 401) {
                this.handleAuthError();
            } else {
                this.handleConnectionError();
            }
        });
    };

    getMessages = () => {
        Message.getAll((messageArray, status, res) => {
            if (status === 200){
                this.setState({ messages: messageArray });
                var listDiv = document.getElementById("messageList");
                listDiv.scrollTop = listDiv.scrollHeight;
            } else if (status === 401) {
                this.handleAuthError();
            } else {
                this.handleConnectionError();
            }
        });
    };

    getNewMessages = () => {
        if (this.state.messages.length > 0){
            var lastId = this.state.messages[this.state.messages.length - 1].Id;
            Message.getSince(lastId, (newMessages, status, res) => {
                if (status === 200){
                    if (newMessages.length > 0){
                        var allMessages = this.state.messages.concat(newMessages);
                        this.setState({ messages: allMessages });
                        var listDiv = document.getElementById("messageList");
                        listDiv.scrollTop = listDiv.scrollHeight;
                    }
                } else if (status === 401) {
                    this.handleAuthError();
                } else {
                    this.handleConnectionError();
                }
            });
        } else {
            this.getMessages();
        }
    };

    handleTextChange = (e) => {
        this.text = e.target.value;
    };

    handleSendClick = () => {
        if (this.text.length === 0) return;
        var message = new Message(this.text, null, null, null);
        document.getElementById("messageTextField").value="";
        this.text="";
        message.send((newMessage, status, res) => {
            if (status === 200){
                this.getNewMessages();
            } else if (status === 401) {
                this.handleAuthError();
            } else {
                this.handleConnectionError();
            }
        });
    };

    handleAuthError = () => {
        this.setState({ authError: true });
    };

    handleConnectionError = () => {
        this.setState({ connectionError: true });
    };

    render = () => {
        return (
            <div style={{display: "flex", flexDirection: "column", height: "100%"}}>
                <div style={{ flex: 1 }}>
                    <Overlay 
                        visible={this.state.authError}
                        background="rgba(128, 222, 234, 0.8)">
                        <h1>Dostop zavrnjen</h1>
                        <p>Strežnik vam je zavrnil dostop. 
                        Lahko je prišlo do napake pri avtentikaciji ali 
                        pa je bil vaš račun onemogočen.</p>
                        <FlatButton label="Nazaj na prijavo" onClick={() => { browserHistory.push('/login'); }} />
                    </Overlay>
                    <Overlay 
                        visible={this.state.connectionError}
                        background="rgba(128, 222, 234, 0.8)">
                        <h1>Napaka v povezavi</h1>
                        <p>Zahteva ni bila uspešna iz neznanega razloga. Poskusite ponovno.</p>
                        <FlatButton label="V redu" onClick={() => { this.setState({connectionError: false}); }} />
                    </Overlay>
                    <div style={containerStyle}>
                        <Paper zDepth={1} style={messageListStyle}>
                            <MessageList 
                                users={this.state.users}
                                messages={this.state.messages}
                                me={this.state.me} />
                        </Paper>
                        <Paper className="hideOnSmallScreen" zDepth={1} style={userListStyle} >
                            <UserList 
                                users={this.state.users} 
                                me={this.state.me} />
                        </Paper>
                    </div>
                </div>
                <Paper zDepth={1} style={messageContainerStyle}>
                    <TextField
                        id="messageTextField"
                        style={{flex: 1}}
                        onChange={this.handleTextChange}
                        hintText="Sporočilo"
                        multiLine={true}
                        rows={2}
                        rowsMax={2}/>
                    <FlatButton
                        style={{height: "4rem", marginLeft: "1rem"}}
                        icon={<ContentSend />}/>
                </Paper>
            </div>
        );
    };
}

export default ChatRoom;