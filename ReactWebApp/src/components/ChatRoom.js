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

const containerStyle = {
    height: "calc(100vh - 7rem - 64px)",
    display: "flex",
    flexDirection: "row"
}

class ChatRoom extends React.Component {
    text = "";
    getUserDataInterval = null;
    getMessagesInterval = null;

    constructor(props) {
        super(props);
        this.state = {
            users: {},
            messages: [],
            me: User.getMe(),
            authError: false,
            connectionError: false
        };
    }

    componentDidMount = () => {
        this.setState({
            users: User.getAll(),
            messages: Message.getAll()
        })
        this.scrollToBottom();
        
        this.getUserData();
        this.getMessages();
        this.getUserDataInterval = window.setInterval(this.getUserData, 60000);
        this.getMessagesInterval = window.setInterval(this.getMessages, 5000);

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
        if (this.getMessagesInterval !== null)
            window.clearInterval(this.getMessagesInterval);
    }

    scrollToBottom = () => {
        try {
            var listDiv = document.getElementById("messageList");
            listDiv.scrollTop = listDiv.scrollHeight;
        } catch (error) {
            //
        }
    }

    getUserData = () => {
        User.getMe((user, status, res) => {
            if (user != null) {
                this.setState({ me: user });
            } else if (status === 401) {
                this.handleAuthError();
            } else {
                this.handleConnectionError();
            }
        });
        User.getAll((users, status, res) => {
            if (status === 200) {
                this.setState({ users: users });
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
                this.scrollToBottom();
                for (var i = 0; i < messageArray.length; i++){
                    var m = messageArray[i];
                    if (User.get(m.Username) == null){
                        User.get(m.Username, (user, status, res) => {
                            this.setState({ messages: messageArray });
                        });
                    }
                }
            } else if (status === 401) {
                this.handleAuthError();
            } else {
                this.handleConnectionError();
            }
        });
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