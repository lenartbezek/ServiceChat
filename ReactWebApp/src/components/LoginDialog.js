import React from 'react';
import Dialog from 'material-ui/Dialog';
import FlatButton from 'material-ui/FlatButton';
import RaisedButton from 'material-ui/RaisedButton';
import TextField from 'material-ui/TextField';
import Overlay from './Overlay';

import * as auth from '../auth';

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

const loginButtonStyle = {
    width: "10rem"
}

class LoginDialog extends React.Component {
    username = "";
    password = "";

    state = {
        open: false,
        error: null,
        usernameError: "",
        passwordError: "",
        waitingForResponse: false
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

    handleLoginChatClick = () => {
        if (this.checkRequiredFields()){
            this.setState({ waitingForResponse: true });
            auth.login(this.username, this.password, (status, response) =>{
                this.setState({ waitingForResponse: false });
                if (status === 200 && response.Success){
                    this.handleClose();
                    this.props.onLogin();
                } else {
                    switch (response.Error){
                        case 'UserNotFound':
                            this.setState({ usernameError: "Ta račun ne obstaja" }); break;
                        case 'InvalidPassword':
                            this.setState({ passwordError: "Napačno geslo" }); break;
                        default:
                            this.setState({ error: status });
                    }
                }
            });
        }
    };

    handleLoginAdminClick = () => {
        if (this.checkRequiredFields()){
            this.setState({ waitingForResponse: true });
            auth.login(this.username, this.password, (status, response) =>{
                this.setState({ waitingForResponse: false });
                if (status === 200 && response.Success){
                    if (response.Admin){
                        this.handleClose();
                        this.props.onLoginAdmin();
                    } else {
                        this.setState({ usernameError: "Ta račun nima administratorskih pravic" });
                    }
                } else {
                    switch (response.Error){
                        case 'UserNotFound':
                            this.setState({ usernameError: "Ta račun ne obstaja" }); break;
                        case 'InvalidPassword':
                            this.setState({ passwordError: "Napačno geslo" }); break;
                        default:
                            this.setState({ error: status });
                    }
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

    handleCloseErrorOverlay = () => {
        this.setState({ error: null });
    };

    render = () => {
        return (
        <div>
            <RaisedButton 
                style={loginButtonStyle}
                label="Prijava"
                onClick={this.handleOpen}
                primary={true}
                disabled={this.props.disabled}/>
            <Dialog
            title="Prijava"
            actions={[]}
            modal={false}
            open={this.state.open}
            contentStyle={dialogStyle}
            onRequestClose={this.handleClose}>
                <Overlay visible={this.state.error !== null}>
                    <strong>Napaka</strong>
                    <p>{this.state.error}}</p>
                    <FlatButton label="V redu" onClick={this.handleCloseErrorOverlay} />
                </Overlay>
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
                    onClick={this.handleLoginChatClick}
                    disabled={this.state.waitingForResponse}
                    label="Prijava v klepet" primary={true} /><br/>
                <FlatButton
                    style={buttonStyle}
                    onClick={this.handleLoginAdminClick}
                    disabled={this.state.waitingForResponse}
                    label="Administratorske strani" secondary={true} /><br/>
            </Dialog>
        </div>
        );
    };
};

LoginDialog.defaultProps = {
    onLogin: () => {},
    onLoginAdmin: () => {},
    disabled: false,
};

export default LoginDialog;
