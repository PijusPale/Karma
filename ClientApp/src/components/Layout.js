import React, { useState } from 'react';
import { Container } from 'reactstrap';
import { NavMenu } from './NavMenu';
import { Chat } from './Chat';

export const Layout = (props) => {
  const [showChat, setShowChat] = useState(false);

  function toggleChat () {
    setShowChat(!showChat);
  }

  return(
    <div>
      <NavMenu toggleChat={toggleChat} />
      <Container>
        {props.children}
      </Container>
      {showChat && <Chat />}
    </div>
  );
}