import React from 'react';
import MainAppBar from './MainAppBar';

import { withRouter } from 'react-router'

const fullscreenWrapperStyle = {
    background: "#80DEEA",
    position: "fixed",
    width: "100%",
    height: "100%",
    top: 0,
    left: 0
}

const containerStyle = {
    maxWidth: "100%",
    height: "calc(100% - 64px)",
    margin: "auto",
    width: "60rem"
}

class Index extends React.Component {
    render = () => {
        return (
            <div style={fullscreenWrapperStyle}>
                <MainAppBar />
                <div style={containerStyle}>
                    {this.props.children}
                </div>
            </div>
        );
    }
}

export default withRouter(Index);