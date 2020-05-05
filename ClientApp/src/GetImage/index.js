import React, { Component } from 'react';
import './image.css'

function arrayBufferToBase64(buffer) {
    var binary = '';
    var bytes = [].slice.call(new Uint8Array(buffer));
  
    bytes.forEach((b) => binary += String.fromCharCode(b));
  
    return window.btoa(binary);
  };

class Image extends Component {
    constructor(props) {
        super(props);
        this.state = {
            img: ''
        };
    };
    arrayBufferToBase64(buffer) {
        var binary = '';
        var bytes = [].slice.call(new Uint8Array(buffer));
        bytes.forEach((b) => binary += String.fromCharCode(b));
        return window.btoa(binary);
    };
    componentDidMount() {
        fetch(`familytree/img/${this.props.image}`)
        .then((response) => {
        response.arrayBuffer().then((buffer) => {
            var base64Flag = 'data:image/jpeg;base64,';
            var imageStr = arrayBufferToBase64(buffer);
            this.setState({
                img: base64Flag + imageStr
            })
        })
        });
    }
    render() {
        const {img} = this.state;
        return (
            <div className="image">
                <img 
                    src={img}
                    alt='Helpful alt text'
                    style={{height: "128px" }}/>
            </div>
        )
    }
}
export default Image;