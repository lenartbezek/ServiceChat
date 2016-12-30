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

const titleStyle = {
    top: "-2rem",
    fontSize: "4rem",
    fontWeight: 100,
    width: "100%",
    color: "white",
    background: "#039BE5",
    textAlign: "center",
    padding : "2rem"
}

const containerStyle = {
    height: "calc(100vh - 7rem)",
    display: "flex",
    flexDirection: "row"
}

class ChatRoom extends React.Component {
    text = "";

    state = {
        messages: [],
        users: null,
        me: null,
        loaded: false,
        authError: false,
        connectionError: false
    };

    componentDidMount = () => {
        this.getUserData();
        this.getNewMessages();
        window.setInterval(this.getUserData, 15000);
        window.setInterval(this.getNewMessages, 5000);

        var rc = this;
        document.getElementById("messageTextField")
            .addEventListener("keyup", function(event) {
            if (event.keyCode === 13) {
                rc.handleSendClick();
            }
        });
    };

    getUserData = () => {
        var checkIfLoaded = () => {
            this.setState({ loaded: this.state.users !== null && this.state.me !== null });
        };
        User.me((user, status, res) => {
            if (status === 200) {
                this.setState({ 
                    me: user
                });
                checkIfLoaded();
            } else if (status === 401) {
                this.handleAuthError();
            } else {
                this.handleConnectionError();
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
                this.setState({ 
                    users: userDict
                });
                checkIfLoaded();
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
            <div>
                <Overlay 
                    visible={!this.state.loaded}
                    background="white">
                    <span style={titleStyle}>ServiceChat</span>
                    <p>Povezovanje ...</p>
                </Overlay>
                <Overlay visible={this.state.authError}>
                    <h1>Dostop zavrnjen</h1>
                    <p>
                    Strežnik vam je zavrnil dostop. 
                    Lahko je prišlo do napake pri avtentikaciji ali 
                    pa je bil vaš račun onemogočen.
                    </p>
                    <FlatButton label="Nazaj na prijavo" onClick={() => { browserHistory.push('/login'); }} />
                </Overlay>
                <Overlay visible={this.state.connectionError}>
                    <h1>Napaka v povezavi</h1>
                    <p>
                    Zahteva ni bila uspešna iz neznanega razloga. Poskusite ponovno.
                    </p>
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
                <Paper zDepth={1} style={messageContainerStyle}>
                    <TextField
                        id="messageTextField"
                        style={{flex: 1}}
                        onChange={this.handleTextChange}
                        hintText="Sporočilo"/>
                    <FlatButton
                        style={{height: "3rem", marginLeft: "1rem"}}
                        icon={<ContentSend />}/>
                </Paper>
            </div>
        );
    };
}

export default ChatRoom;