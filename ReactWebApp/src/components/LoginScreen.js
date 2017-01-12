import React from 'react';
import LoginDialog from './LoginDialog';
import RegisterDialog from './RegisterDialog';
import FlatButton from 'material-ui/FlatButton';
import Paper from 'material-ui/Paper';
import { browserHistory } from 'react-router';
import { titleStyle, overlayStyle } from '../styles'

import User from '../models/User';

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
    width: "12rem",
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
        if (User.loggedIn()){
            User.getMe((user, status, res) => {
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
        User.logout(() => {
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
                    <FlatButton 
                        label="Nazaj v klepetalnico" 
                        style={buttonStyle}
                        disabled={!this.state.showOptions}
                        onClick={this.handleLoginChatSuccess} />
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
                <Paper style={titleStyle}>ServiceChat</Paper>
                <span style={{...messageStyle, ...fadeStyle}}>{this.state.message}</span>
                {options}
            </div>
        );
    };
}

export default LoginScreen;