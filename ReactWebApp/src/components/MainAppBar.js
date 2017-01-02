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

import * as auth from '../auth';

class MainAppBar extends React.Component {
    state = {
        drawerOpen: false
    }

    handleDrawerOpen = () => {
        this.setState({ drawerOpen: true });
    }

    handleDrawerClose = () => {
        this.setState({ drawerOpen: false });
    }

    handleLogoutClick = () => {
        auth.logout(() => { browserHistory.push('/login'); })
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
                        <p>Prijavljeni ste kot <br /><b>{auth.getUser().DisplayName}</b></p>
                    </div>
                    <MenuItem 
                        primaryText="Odjavi se" 
                        leftIcon={<IconAccount />}
                        onClick={this.handleLogoutClick}/>
                    <Divider />
                    <MenuItem 
                        primaryText="Klepetalnica" 
                        leftIcon={<IconChat />}
                        onClick={() => { browserHistory.push('/'); }}/>
                    <MenuItem 
                        primaryText="Administratorske strani" 
                        leftIcon={<IconAdmin />}
                        onClick={() => { browserHistory.push('/admin'); }}/>
                </Drawer>
            </div>
        );
    }
}

export default MainAppBar;