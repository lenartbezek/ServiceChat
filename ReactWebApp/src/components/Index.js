import React from 'react';
import { browserHistory, Link, withRouter } from 'react-router'

const containerStyle = {
    maxWidth: "60rem",
    margin: "auto"
}

class Index extends React.Component {
    render = () => {
        return (
            <div style={containerStyle}>
                {this.props.children}
            </div>
        );
    }
}

export default withRouter(Index);