import React from 'react';
import Dialog from 'material-ui/Dialog';
import FlatButton from 'material-ui/FlatButton';
import RaisedButton from 'material-ui/RaisedButton';
import TextField from 'material-ui/TextField';
import Overlay from './Overlay';

import User from '../models/User';

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

const registerButtonStyle = {
    width: "10rem"
};

function isAsciiOnly(str) {
    for (var i = 0; i < str.length; i++)
        if (str.charCodeAt(i) > 127)
            return false;
    return true;
};

function containsTwoNumeric(str){
    var numericCharCount = 0;
    for (var i = 0, len = str.length; i < len; i++) {
        var c = str.charAt(i);
        if (c.match(/[0-9]/i)) numericCharCount++;
    }
    return numericCharCount >= 2;
};

function containsUppercase(str){
    var uppercaseCharCount = 0;
    for (var i = 0, len = str.length; i < len; i++) {
        var c = str.charAt(i);
        if (c.match(/[A-Z]/i)) uppercaseCharCount++;
    }
    return uppercaseCharCount > 0;
};

function containsSpecial(str){
    return (str.indexOf("?") !== -1 ||
           str.indexOf(".") !== -1 ||
           str.indexOf("*") !== -1 ||
           str.indexOf("!") !== -1 ||
           str.indexOf(":") !== -1);
};

class RegisterDialog extends React.Component {
    name = "";
    username = "";
    password = "";
    repeat = "";

    state = {
        open: false,
        success: false,
        error : null,
        nameError: "",
        usernameError: "",
        passwordError: "",
        repeatError: ""
    };

    checkRequiredFields(){
        var usernameError = "";
        var passwordError = "";
        if (this.username.length < 1)
            usernameError = "Vnesite uporabniško ime";
        if (this.password.length < 1)
            passwordError = "Vnesite geslo";
        this.setState({ usernameError: usernameError, passwordError: passwordError });
        return this.username.length > 0 && this.password.length > 0;
    };

    handleRegisterClick = () => {
        this.setState({ waitingForResponse: true });
        var newUser = new User(this.username, this.name);
        newUser.register(this.password, (user, status, message) => {
            if (user != null){
                this.handleRegisterSuccess(user);
            } else {
                this.handleRegisterError(status, message);
            }
        });
    };

    handleRegisterSuccess = () => {
        this.setState({ success: true, waitingForResponse: false });
    };

    handleRegisterError = (status, message) => {
        var error = null;
        var nameError = "";
        var usernameError = "";
        var passwordError = "";
        var repeatError = "";
        switch (message){
            case "InvalidUsername":
                usernameError = "Uporabniško sme vsebovati le ASCII znake in biti dolgo najmanj štiri znake"; break;
            case "InvalidPassword":
                passwordError = "Geslo je premalo zajebano"; break;
            case "DuplicateUsername":
                usernameError = "To uporabniško ime je že zasedeno"; break;
            default:
                error = status;
        }
        this.setState({
            error: error,
            nameError: nameError,
            usernameError: usernameError,
            passwordError: passwordError,
            repeatError: repeatError,
            waitingForResponse: false
        });
    };

    handleNameFieldChange = (e) => {
        this.name = e.target.value;
    };

    handleUsernameFieldChange =(e) => {
        this.username = e.target.value;
        if (this.username.length < 4){
            this.setState({usernameError: "Uporabniško ime mora biti dolgo vsaj 4 znake"});
        } else if (!isAsciiOnly(this.username)){
            this.setState({usernameError: "Uporabniško lahko vsebuje le ASCII znake"});
        } else {
            this.setState({usernameError: ""});
        }
    };

    handlePasswordFieldChange = (e) => {
        this.password = e.target.value;
        if (!containsUppercase(this.password)){
            this.setState({passwordError: "Geslo mora vsebovati vsaj eno veliko črko"});
        } else if (!containsTwoNumeric(this.password)) {
            this.setState({passwordError: "Geslo mora vsebovati vsaj dve številki"});
        } else if (!containsSpecial(this.password)) {
            this.setState({passwordError: "Geslo mora vsebovati vsaj en poseben znak (?!*:.)"});
        } else if (this.password.length < 8) {
            this.setState({passwordError: "Geslo mora biti dolgo vsaj 8 znakov"});
        } else {
            this.setState({passwordError: ""});
        }
        if (this.repeat.length > 0 && this.password !== this.repeat){
            this.setState({repeatError: "Gesli se ne ujemata"});
        }
    };

    handleRepeatFieldChange = (e) => {
        this.repeat = e.target.value;
        if (this.repeat !== this.password){
            this.setState({repeatError: "Gesli se ne ujemata"});
        } else {
            this.setState({repeatError: ""});
        }
    };

    handleOpen = () => {
        this.setState({ open: true });
    };

    handleClose = () => {
        this.setState({
            open: false,
            success: false,
            error : null,
            nameError: "",
            usernameError: "",
            passwordError: "",
            repeatError: ""
        });
    };

    handleCloseErrorOverlay = () => {
        this.setState({ error: null });
    }

    render = () => {
        const invalid = !(this.state.nameError.length === 0 &&
                        this.state.usernameError.length === 0 &&
                        this.state.passwordError.length === 0 &&
                        this.state.repeatError.length === 0 &&
                        this.username.length !== 0 &&
                        this.password.length !== 0 &&
                        this.repeat.length !== 0 &&
                        this.password === this.repeat);
        return (
        <div>
            <RaisedButton
                style={registerButtonStyle}
                label="Registracija"
                onClick={this.handleOpen}
                secondary={true}
                disabled={this.props.disabled}/>
            <Dialog
                title="Registracija"
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
                <Overlay visible={this.state.success}>
                    <strong>Registracija uspešna</strong>
                    <p>Sedaj se lahko prijavite.</p>
                    <FlatButton label="Zapri" onClick={this.handleClose} />
                </Overlay>
                <TextField
                    style={inputStyle}
                    floatingLabelText="Ime in priimek"
                    errorText={this.state.nameError}
                    onChange={this.handleNameFieldChange}
                /><br />
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
                <TextField
                    style={inputStyle}
                    floatingLabelText="Ponovite geslo"
                    type="password"
                    errorText={this.state.repeatError}
                    onChange={this.handleRepeatFieldChange}
                /><br />
                <RaisedButton
                    style={buttonStyle}
                    onClick={this.handleRegisterClick}
                    disabled={invalid || this.state.waitingForResponse}
                    label="Ustvari nov račun" primary={true} />
            </Dialog>
        </div>
        );
    };
};

RegisterDialog.defaultProps = {
    disabled: false
};

export default RegisterDialog;
