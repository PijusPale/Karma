import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { NavMenu } from './NavMenu';
import { Chat } from './Chat';

export class Layout extends Component {
  static displayName = Layout.name;
  constructor(props){
    super(props);
    this.toggleChat = this.toggleChat.bind(this);
    this.state = {
      showChat: false
    }
  }

  toggleChat() {
    this.setState({ showChat: !this.state.showChat});
  }

  render() {
    return (
      <div>
        <NavMenu toggleChat={this.toggleChat}/>
        <Container>
          {this.props.children}
        </Container>
        {this.state.showChat && <Chat/>}
      </div>
    );
  }
}
