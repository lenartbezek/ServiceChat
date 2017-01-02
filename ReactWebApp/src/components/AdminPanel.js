import React from 'react';
import Paper from 'material-ui/Paper';
import {Table, TableBody, TableHeader, TableHeaderColumn, TableRow, TableRowColumn} from 'material-ui/Table';
import Checkbox from 'material-ui/Checkbox';
import FlatButton from 'material-ui/FlatButton';
import RaisedButton from 'material-ui/RaisedButton';
import IconDelete from 'material-ui/svg-icons/action/delete-forever';
import Overlay from './Overlay';
import { browserHistory } from 'react-router';

import User from '../models/User';
import Message from '../models/Message';
import * as auth from '../auth';

class AdminPanelRow extends React.Component {
    constructor(props, context) {
        super(props, context);

        this.state = {
            user: props.user,
            open: false,
            pendingDelete: false,
            pendingEdit: false
        };
    }

    handleDeleteClick = () => {
        this.setState({ pendingDelete: true });
        this.state.user.delete((user, status, res) => {
            if (status === 200){
                this.props.onDelete(this.state.user);
            } else {
                this.setState({ pendingDelete: false });
                this.props.onError(status);
            }
        });
    }

    handleAdminToggle = (object, checked) => {
        var tmpUser = this.state.user;
        tmpUser.Admin = checked;
        this.setState({ pendingEdit: true, user: tmpUser });
        this.state.user.edit((user, status, res) => {
            if (status === 200){
                this.props.onEdit(this.state.user);
                this.setState({ pendingEdit: false });
            } else {
                var tmpUser = this.state.user;
                tmpUser.Admin = !checked;
                this.setState({ pendingEdit: false, user: tmpUser });
                this.props.onError(status);
            }
        });
    }

    render = () => {
        return (
            <TableRow>
                <TableRowColumn>{this.state.user.Username}</TableRowColumn>
                <TableRowColumn>{this.state.user.DisplayName}</TableRowColumn>
                <TableRowColumn>{this.props.messageCount}</TableRowColumn>
                <TableRowColumn>
                    <Checkbox 
                        checked={this.state.user.Admin}
                        disabled={this.props.me.Username === this.state.user.Username || 
                                  this.state.pendingEdit || 
                                  this.state.pendingDelete}
                        onCheck={this.handleAdminToggle}
                    />
                </TableRowColumn>
                <TableRowColumn>
                    <FlatButton 
                        icon={<IconDelete />} 
                        disabled={this.props.me.Username === this.state.user.Username || 
                                  this.state.pendingDelete}
                        onClick={this.handleDeleteClick}
                    />
                </TableRowColumn>
            </TableRow>
        );
    }
}

AdminPanelRow.defaultProps = {
    messageCount: 0,
    onEdit: (user) => {},
    onDelete: (user) => {},
    onError: (status) => {}
}

class AdminPanel extends React.Component {
    constructor(props, context) {
        super(props, context);

        this.state = {
            users: [],
            messages: [],
            me: auth.getUser(),
            authError: false,
            connectionError: false
        };
    };

    componentDidMount = () => {
        if (this.state.me.Admin){
            this.getUserData();
            this.getMessages();
        }
    };

    handleRefreshClick = () => {
        if (this.state.me.Admin){
            this.getUserData();
            this.getMessages();
        }
    };

    getUserData = () => {
        auth.refreshUser((status, res) => {
            if (status === 200 && res.Status){
                this.setState({ me: auth.getUser() });
            }
        });
        User.getAll((userArray, status, res) => {
            if (status === 200) {
                this.setState({ users: userArray, loaded: true});
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
            } else if (status === 401) {
                this.handleAuthError();
            } else {
                this.handleConnectionError();
            }
        });
    };

    handleUserDelete = (user) => {
        this.setState({ users: this.state.users.filter((u) => u.Username !== user.Username)})
    };

    handleUserEdit = (user) => {

    };

    handleTaskError = (status) => {
        if (status === 401){
            this.setState({ authError: true });
        } else {
            this.setState({ connectionError: true });
        }
    };

    render = () => {
        var list = this.state.users.map((user) => {
            return <AdminPanelRow 
                        key={user.Username} 
                        user={user}
                        me={this.state.me}
                        messageCount={this.state.messages.filter(
                            (m) => {return m.Username === user.Username}).length}
                        onEdit={this.handleUserEdit}
                        onDelete={this.handleUserDelete}
                        onError={this.handleTaskError}
                    />;
        });
        return (
            <div>
            <Overlay
                visible={!this.state.me.Admin}
                background="rgba(128, 222, 234, 0.8)">
                <h1>Dostop zavrnjen</h1>
                <p>Samo administratorji imajo dostop do administratorske konzole.</p>
            </Overlay>
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
            <Paper style={{ margin: "0.5rem"}}>
                <div style={{ height: 36, padding: "0.5rem", display: "flex"}}>
                    <p style={{ marginLeft: "3rem" }}>Administratorska konzola</p>
                    <div style={{ flex: 1, textAlign: "right"}}>
                    <RaisedButton
                        label="Osveži"
                        onClick={this.handleRefreshClick}
                        primary={true}
                    />
                    </div>
                </div>
                <Table
                    height={"calc(100vh - 174px - 1rem)"}
                    fixedHeader={true}
                    selectable={false}>
                    <TableHeader 
                        displaySelectAll={false}
                        adjustForCheckbox={false}>
                        <TableRow>
                            <TableHeaderColumn>Uporabniško ime</TableHeaderColumn>
                            <TableHeaderColumn>Ime in priimek</TableHeaderColumn>
                            <TableHeaderColumn>Število sporočil</TableHeaderColumn>
                            <TableHeaderColumn>Administrator</TableHeaderColumn>
                            <TableHeaderColumn>Brisanje</TableHeaderColumn>
                        </TableRow>
                    </TableHeader>
                    <TableBody displayRowCheckbox={false}>
                        {list}
                    </TableBody>
                </Table>
            </Paper>
            </div>
        );
    }
}

export default AdminPanel;