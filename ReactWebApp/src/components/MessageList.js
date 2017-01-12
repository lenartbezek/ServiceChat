import React from 'react';
import Avatar from 'material-ui/Avatar';
import Chip from 'material-ui/Chip';
import Subheader from 'material-ui/Subheader';
import Divider from 'material-ui/Divider';
import {List, ListItem} from 'material-ui/List';
import ActionAccountCircle from 'material-ui/svg-icons/action/account-circle';
import moment from 'moment';
import { listStyle } from '../styles';

moment.locale("sl-SI");

const messageChipBarStyle = {
    display: "flex",
    flexDirection: "row"
}

const timeStyle = {
    color: "grey",
    fontSize: "0.7em",
    textAlign: "right",
    flex: 1
}

class MessageList extends React.Component {
    render = () => {
        if (this.props.messages.length === 0)
            return (
                <div>
                    <Subheader>Sporočila</Subheader>
                    <Divider />
                    <List>
                        <p style={{padding: "3rem"}}>Čakanje na sporočila ...</p>
                    </List>
                </div>
            );
        var list = [];
        this.props.messages.forEach((message) => {
            const name = this.props.users.hasOwnProperty(message.Username)
                ? this.props.users[message.Username].DisplayName
                : "...";
            var item = (
                <ListItem key={message.Id}>
                    <div style={messageChipBarStyle}>
                        <Chip>
                            <Avatar icon={<ActionAccountCircle />} />
                            {name}
                        </Chip>
                        <span style={timeStyle}>{moment(message.Time).fromNow()}</span>
                    </div>
                    <p>{message.Text}</p>
                </ListItem>
            );
            list.push(item);
        });
        return (
            <div style={{height: "100%"}}>
                <Subheader>Sporočila</Subheader>
                <Divider />
                <div style={listStyle} id="messageList">
                    <List>{list}</List>
                </div>
            </div>
        );
    }
}

MessageList.defaultProps = {
    users: {},
    messages: []
};

export default MessageList;