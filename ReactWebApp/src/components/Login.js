import React from 'react';
import Dialog from 'material-ui/Dialog';
import FlatButton from 'material-ui/FlatButton';
import RaisedButton from 'material-ui/RaisedButton';
import TextField from 'material-ui/TextField';

import {apiUrl} from '../config.js';

const dialogStyle = {
    maxWidth: '25rem',
    textAlign: "center"
};

const inputStyle = {
    margin: "auto",
    marginTop: "-1rem",
    marginBottom: "2rem"
};

const buttonStyle = {
    width: "16rem",
    margin: "auto",
    marginTop: "0.5rem"
};

function login(username, password, onSuccess, onError){
    var xhr = new XMLHttpRequest();
    xhr.open('POST', apiUrl+'/Login');
    xhr.setRequestHeader("Content-type", "application/json");
    xhr.onload = function() {
        var response = JSON.parse(xhr.response);
        if (response.Success) {
            onSuccess(response);
        }
        else {
            onError(response);
        }
    };
    xhr.send(JSON.stringify({ Username: username, Password: password }));
};

class Login extends React.Component {
    username = "";
    password = "";

    state = {
        open: false,
        usernameError: "",
        passwordError: ""
    };

    checkRequiredFields = () => {
        var usernameError = "";
        var passwordError = "";
        if (this.username.length < 1)
            usernameError = "Vnesite uporabniško ime";
        if (this.password.length < 1)
            passwordError = "Vnesite geslo";
        this.setState({ usernameError: usernameError, passwordError: passwordError });
        return this.username.length > 0 && this.password.length > 0;
    };

    loginChat = () => {
        if (this.checkRequiredFields()){
            login(this.username, this.password,
                (response) => {
                    this.props.onLogin();
                },
                (response) => {
                    switch (response.Error){
                        case 'UserNotFound':
                            this.setState({ usernameError: "Ta račun ne obstaja" }); break;
                        case 'InvalidPassword':
                            this.setState({ passwordError: "Napačno geslo" }); break;
                        default:
                            alert("Neznana napaka!\n"+JSON.stringify(response));
                    }
                });
        }
    };

    loginAdmin = () => {
        if (this.checkRequiredFields()){
            login(this.username, this.password,
                (response) => {
                    if (response.Admin)
                        this.props.onLoginAdmin();
                    else
                        this.setState({ usernameError: "Ta račun nima administratorskih pravic" });
                },
                (response) => {
                    switch (JSON.parse(response).Error){
                        case 'UserNotFound':
                            this.setState({ usernameError: "Ta račun ne obstaja" }); break;
                        case 'InvalidPassword':
                            this.setState({ passwordError: "Napačno geslo" }); break;
                        default:
                            alert("Neznana napaka!\n"+JSON.stringify(response));
                    }
                });
        }
    };

    handleUsernameFieldChange = (e) => {
        this.username = e.target.value;
        if (this.state.usernameError.length > 0)
            this.setState({ usernameError: "" });
    };

    handlePasswordFieldChange = (e) => {
        this.password = e.target.value;
        if (this.state.passwordError.length > 0)
            this.setState({ passwordError: "" });
    };

    handleOpen = () => {
        this.setState({ open: true });
    };

    handleClose = () => {
        this.setState({ open: false });
    };

    render = () => {
        const actions = [];
        return (
        <div>
            <RaisedButton label="Prijava" onClick={this.handleOpen} primary={true} />
            <Dialog
            title="Prijava"
            actions={actions}
            modal={false}
            open={this.state.open}
            contentStyle={dialogStyle}
            onRequestClose={this.handleClose}>
                <TextField
                    style={inputStyle}
                    floatingLabelText="Uporabniško ime"
                    errorText={this.state.usernameError}
                    onChange={this.handleUsernameFieldChange}
                /><br />
                <TextField
                    style={inputStyle}
                    floatingLabelText="Geslo"
                    type="password"
                    errorText={this.state.passwordError}
                    onChange={this.handlePasswordFieldChange}
                /><br />
                <RaisedButton
                    style={buttonStyle}
                    onClick={this.loginChat}
                    label="Prijava v klepet" primary={true} /><br/>
                <FlatButton
                    style={buttonStyle}
                    onClick={this.loginAdmin}
                    label="Administratorske strani" secondary={true} /><br/>
            </Dialog>
        </div>
        );
    };
};

export default Login;
