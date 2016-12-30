import React from 'react';
import LoginDialog from './LoginDialog';
import RegisterDialog from './RegisterDialog';
import { browserHistory } from 'react-router';
import auth from '../auth.js';
import {apiUrl} from '../config.js';

const overlayStyle = {
    position: "fixed",
    width: "100%",
    height: "100%",
    top: 0,
    left: 0,
    display: "flex",
    flexDirection: "column",
    justifyContent: "center",
    alignItems: "center"
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

const messageStyle = {
    margin: "2rem",

}

const buttonRowStyle = {
    display: "flex",
    flexDirection: "row",
    justifyContent: "center",
    alignItems: "center"
}

const fadeOutStyle = {
    opacity:0,
    transition: "opacity 0.5s linear"
}

const fadeInStyle = {
    opacity:1,
    transition: "opacity 0.5s linear"
}

const spacerStyle = {
    width: "1rem",
}

class LoginScreen extends React.Component {
    state = {
        connected: false,
        message: "Za obisk te spletne strani morate biti prijavljeni."
    };

    componentDidMount = () => {
        setTimeout( () => { this.setState({ connected: true }); }, 500);
    };

    handleLoginChatSuccess = () => {
        browserHistory.push('/');
    };

    handleLoginAdminSuccess = () => {
        browserHistory.push('/admin');
    }

    handleLoginError = () => {
        
    };

    render = () => {
        var fadeStyle = fadeOutStyle;
        if (this.state.connected){
            fadeStyle = fadeInStyle;
        }
        return (
            <div style={overlayStyle}>
                <span style={titleStyle}>ServiceChat</span>
                <span style={{...messageStyle, ...fadeStyle}}>{this.state.message}</span>
                <div style={{...buttonRowStyle, ...fadeStyle}}>
                    <LoginDialog
                        onLogin={this.handleLoginChatSuccess}
                        onLoginAdmin={this.handleLoginAdminSuccess}
                        disabled={!this.state.connected}/>
                    <div style={spacerStyle}></div>
                    <RegisterDialog 
                        disabled={!this.state.connected}/>
                </div>
            </div>
        );
    };
}

export default LoginScreen;