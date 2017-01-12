import React from 'react';
import {List, ListItem} from 'material-ui/List';
import Subheader from 'material-ui/Subheader';
import Divider from 'material-ui/Divider';
import Avatar from 'material-ui/Avatar';
import ActionAccountCircle from 'material-ui/svg-icons/action/account-circle';
import NavigationArrowBack from 'material-ui/svg-icons/navigation/arrow-back';

import { listStyle } from '../styles';

class UserList extends React.Component {
    render = () => {
        var list = [];
        for (var username in this.props.users) {
            if (this.props.users.hasOwnProperty(username)) {
                var user = this.props.users[username];
                var you = this.props.me !== null && this.props.me.Username === username
                    ? <NavigationArrowBack/>
                    : null;
                var item = (
                    <ListItem
                        key={username}
                        leftAvatar={<Avatar icon={<ActionAccountCircle />} />}
                        rightIcon={you}
                        primaryText={user.DisplayName}
                        secondaryText={user.Admin ? 'Administrator' : ''}/>
                );
                list.push(item);
            }
        }
        return (
            <div style={{height: "100%"}}>
                <Subheader>Udeleženi</Subheader>
                <Divider />
                <div style={listStyle}>
                    <List>{list}</List>
                </div>
            </div>   
        );
    }
}

UserList.defaultProps = {
    users: {},
    me: null,
};

export default UserList;