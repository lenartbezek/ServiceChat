import React from 'react';
import AppBar from 'material-ui/AppBar';
import Drawer from 'material-ui/Drawer';
import Divider from 'material-ui/Divider';
import MenuItem from 'material-ui/MenuItem';
import IconChat from 'material-ui/svg-icons/communication/message';
import IconAdmin from 'material-ui/svg-icons/action/supervisor-account';
import IconAccount from 'material-ui/svg-icons/action/account-circle';
import IconClose from 'material-ui/svg-icons/navigation/close';
import IconButton from 'material-ui/IconButton';
import FlatButton from 'material-ui/FlatButton';
import { browserHistory } from 'react-router';
import User from '../models/User';

class MainAppBar extends React.Component {
    state = {
        me: User.getMe(),
        drawerOpen: false
    }

    componentDidMount = () => {
        User.getMe((user, status, res) => {
            if (user != null) this.setState({ me: user });
        });
    }

    handleDrawerOpen = () => {
        this.setState({ drawerOpen: true });
    }

    handleDrawerClose = () => {
        this.setState({ drawerOpen: false });
    }

    handleLogoutClick = () => {
        User.logout(() => { 
            this.handleDrawerClose();
            browserHistory.push('/login');
        })
    }

    render = () => {
        return (
            <div>
                <AppBar
                    title={<span style={{cursor: 'pointer'}}>ServiceChat</span>}
                    onLeftIconButtonTouchTap={this.handleDrawerOpen}
                    iconElementRight={<FlatButton onClick={this.handleLogoutClick} label="Odjava" />}
                />
                <Drawer open={this.state.drawerOpen}>
                    <IconButton tooltip="Zapri" onClick={this.handleDrawerClose}>
                        <IconClose />
                    </IconButton>
                    <Divider />
                    <div style={{ textAlign: "center"}}>
                        <p>Prijavljeni ste kot <br /><b>{this.state.me.DisplayName}</b></p>
                    </div>
                    <MenuItem 
                        primaryText="Odjavi se" 
                        leftIcon={<IconAccount />}
                        onClick={this.handleLogoutClick}/>
                    <Divider />
                    <MenuItem 
                        primaryText="Klepetalnica" 
                        leftIcon={<IconChat />}
                        onClick={() => { this.handleDrawerClose(); browserHistory.push('/'); }}/>
                    <MenuItem 
                        primaryText="Administratorske strani" 
                        leftIcon={<IconAdmin />}
                        onClick={() => { this.handleDrawerClose(); browserHistory.push('/admin'); }}/>
                </Drawer>
            </div>
        );
    }
}

export default MainAppBar;