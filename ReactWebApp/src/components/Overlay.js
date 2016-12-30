import React from 'react';


const overlayStyle = {
    position: "fixed",
    zIndex: 3,
    width: "100%",
    height: "100%",
    top: 0,
    left: 0,
    transition: "opacity 0.5s ease-in",
    display: "flex",
    flexDirection: "column",
    justifyContent: "center",
    alignItems: "center",
}

class Overlay extends React.Component {
    render = () => {
        var style = JSON.parse(JSON.stringify(overlayStyle));
        style.background = this.props.background;
        if (!this.props.visible){
            style.zIndex = -1;
            style.opacity = 0;
        }
        return (
            <div style={style}>
                {this.props.children}
            </div>
        );
    };
}

Overlay.defaultProps = {
    visible: false,
    background: "rgba(255,255,255,.8)",
};

export default Overlay;