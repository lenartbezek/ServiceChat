import React from 'react';


const overlayStyle = {
    position: "fixed",
    zIndex: 3,
    width: "100%",
    height: "100%",
    top: 0,
    left: 0,
    background: "rgba(255,255,255,.8)",
    transition: "opacity 0.5s ease-in",
    display: "flex",
    flexDirection: "column",
    justifyContent: "center",
    alignItems: "center",
}

const invisibleStyle = {
    position: "fixed",
    zIndex: -1,
    width: "100%",
    height: "100%",
    top: 0,
    left: 0,
    transition: "opacity 0.5s ease-in",
    display: "flex",
    flexDirection: "column",
    justifyContent: "center",
    alignItems: "center",
    opacity: 0
}

class Overlay extends React.Component {
    render = () => {
        var style = invisibleStyle;
        if (this.props.visible)
            style = overlayStyle;
        return (
            <div style={style}>
                {this.props.children}
            </div>
        );
    };
}

export default Overlay;