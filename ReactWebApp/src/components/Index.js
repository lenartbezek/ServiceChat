import React from 'react';

import { withRouter } from 'react-router'

const fullscreenWrapperStyle = {
    position: "fixed",
    width: "100%",
    height: "100%",
    top: 0,
    left: 0
}

const containerStyle = {
    maxWidth: "100%",
    margin: "auto",
    height: "100%",
    width: "60rem"
}

class Index extends React.Component {
    render = () => {
        return (
            <div style={fullscreenWrapperStyle}>
                <div style={containerStyle}>
                    {this.props.children}
                </div>
            </div>
        );
    }
}

export default withRouter(Index);