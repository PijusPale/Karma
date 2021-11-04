import React, { useEffect, useState, useContext } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { UserContext } from '../UserContext';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPaperPlane } from '@fortawesome/free-solid-svg-icons';
import '../chat.css';

//TODO: RegEx expression for messages, save messages and load up on render.

export const Chat = (props) => {
    const [message  , setMessage] = useState('');
    const [messages, setMessages] = useState([]); //array of objects with message.type, message.nick, message.text
    const [hubConnection, setHubConnection] = useState();
    const { user, loggedIn } = useContext(UserContext);

    useEffect(() => {
        if (loggedIn) {
            const connection = new HubConnectionBuilder()
                .withUrl("/ChatHub")
                .build();

            connection.on("ReceiveGroupMessage", (nick, receivedMessage) => receiveMessage(nick, receivedMessage));

            connection.logging = true;
            connection
                .start()
                .then(() => console.log('Connection established.'))
                .catch(err => console.log(`Error while establishing connection: ${err}`))
                .then(() => connection.invoke("AddToGroup", props.groupId))
                .catch(err => console.error(`Error while adding to group: ${err}`));

            setHubConnection(connection);
        }
    }, [loggedIn]);

    const sendMessage = async () => {
        if (message !== '') {
            await hubConnection
                .invoke('SendGroupMessage', props.groupId, user.firstName, message)
                .catch(err => console.error(`Error while sending message: ${err}`));

            setMessage('');
        }
    }

    const receiveMessage = (nick, receivedMessage) => {
        var tempMessage = {
        nickname: nick,
        text: receivedMessage,
        type: user.firstName === nick ? "out" : "in"};
        
        setMessages(messages => messages.concat(tempMessage));
    }

    return (
        loggedIn &&
        <div class="container content">
            <div class="row">
                <div class="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-12">
                    <div class="card">
                        <div class="card-header">Chat</div>
                        <div class="card-body height3">
                            <ul class="chat-list">
                                {messages.map((data, index) =>
                                    <li class={data.type} key={index}>
                                        <div class="chat-img">
                                            <img alt="Avatar" src='images/no-avatar.gif' />
                                        </div>
                                        <div class="chat-body">
                                            <div class="chat-message">
                                                <h5>{data.nickname}</h5>
                                                <p>{data.text}</p>
                                            </div>
                                        </div>
                                    </li>
                                )}
                            </ul>
                            <div class="chat-message clearfix">
                                <div class="input-group flex-nowrap">
                                    <span class="input-group-text" id="addon-wrapping" onClick={sendMessage}><FontAwesomeIcon icon={faPaperPlane} /></span>
                                    <input type="text" class="form-control" placeholder="Enter text here..." value={message} onChange={e => setMessage(e.target.value)} onKeyPress={(t) => t.key === 'Enter' && sendMessage()} aria-label="Message" aria-describedby="addon-wrapping" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};