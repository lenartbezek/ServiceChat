import React from 'react';
import LoginDialog from './LoginDialog';
import RegisterDialog from './RegisterDialog';
import FlatButton from 'material-ui/FlatButton';
import { browserHistory } from 'react-router';

import auth from '../auth.js';

import User from '../models/User';

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
    margin: "2rem"
}

const buttonRowStyle = {
    display: "flex",
    flexDirection: "row",
    justifyContent: "center",
    alignItems: "center"
}

const buttonStyle = {
    width: "16rem",
    margin: "auto",
    marginTop: "0.5rem"
};

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
        showOptions: false,
        loggedIn: false,
        message: "..."
    };

    componentDidMount = () => {
        if (auth.loggedIn()){
            User.me((user, status, res) => {
                if (user != null){
                    this.setState({
                        message: "Prijavljeni ste kot "+user.DisplayName+".",
                        loggedIn: true,
                        showOptions: true
                    });
                } else {
                    this.setState({
                        message: "Napaka pri prijavi. Poskusite ponovno.",
                        loggedIn: false,
                        showOptions: true
                    });
                }
            });
        } else {
            setTimeout( () => {
                this.setState({
                    message: "Za ogled te strani se morate prijaviti.",
                    loggedIn: false,
                    showOptions: true
                });
            }, 500);
        }
    };

    handleLoginChatSuccess = () => {
        browserHistory.push('/');
    };

    handleLoginAdminSuccess = () => {
        browserHistory.push('/admin');
    };

    handleLogOut = () => {
        this.setState({ showOptions: false });
        auth.logout(() => {
            setTimeout( () => {
                this.setState({
                    message: "Uspešno ste se odjavili.",
                    loggedIn: false,
                    showOptions: true
                });
            }, 500);
        });
    };

    handleLoginError = () => {
        
    };

    render = () => {
        var fadeStyle = fadeOutStyle;
        if (this.state.showOptions)
            fadeStyle = fadeInStyle;

        let options = null;
        if (this.state.loggedIn){
            options = (
                <div style={{...buttonRowStyle, ...fadeStyle}}>
                    <FlatButton 
                        label="Odjavi se" 
                        style={buttonStyle}
                        disabled={!this.state.showOptions}
                        onClick={this.handleLogOut} />
                </div>
            );
        } else {
            options = (
                <div style={{...buttonRowStyle, ...fadeStyle}}>
                    <LoginDialog
                        onLogin={this.handleLoginChatSuccess}
                        onLoginAdmin={this.handleLoginAdminSuccess}/>
                    <div style={spacerStyle}></div>
                    <RegisterDialog />
                </div>
            );
        }
        return (
            <div style={overlayStyle}>
                <span style={titleStyle}>ServiceChat</span>
                <span style={{...messageStyle, ...fadeStyle}}>{this.state.message}</span>
                {options}
            </div>
        );
    };
}

export default LoginScreen;