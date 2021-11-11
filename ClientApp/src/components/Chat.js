import React, { useEffect, useState, useContext } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { UserContext } from '../UserContext';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPaperPlane } from '@fortawesome/free-solid-svg-icons';
import '../chat.css';

//TODO: RegEx expression for messages, save messages and load up on render
//TODO: Change from IDs to names when we have a Database for easy union of user table and specific messages table

export const Chat = (props) => {
    const [message  , setMessage] = useState('');
    const [messages, setMessages] = useState([]); //array of objects with message.type, message.nick, message.text
    const [hubConnection, setHubConnection] = useState();
    const [loading, setLoading] = useState(true); // Loading bar while waiting for fetch
    const [firstLoad, setFirstLoad] = useState(false); //True after the first batch of messages is loaded
    const [allLoaded, setAllLoaded] = useState(false);
    const { user, loggedIn } = useContext(UserContext);
    const messagesDiv = document.querySelector("#messages") //gets the div of messsages box

    useEffect( () => {
        if (loggedIn) {
            const connection = new HubConnectionBuilder()
                .withUrl("/ChatHub", { accessTokenFactory: () => localStorage.getItem('token')})
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
            fetchOldMessages(20);
        }
    }, [loggedIn]);

    useEffect(() => {
        if(firstLoad)
        scrollToBottom(messagesDiv);
    }, [firstLoad]);

    const sendMessage = async () => {
        if (message !== '') {
            await hubConnection
                .invoke('SendGroupMessage', props.groupId, user.id, message)
                .catch(err => console.error(`Error while sending message: ${err}`));

            setMessage('');
            scrollToBottom(messagesDiv);
        }
    }

    const receiveMessage = (nick, receivedMessage) => {
        var tempMessage = {
        fromId: nick,
        content: receivedMessage,
        type: user.firstName === nick ? "out" : "in",
        dateSent: Date.now,
        status: 'Delivered'};
        
        setMessages(messages => messages.concat(tempMessage));
        scrollToBottom(messagesDiv);
    }

    const fetchOldMessages = async (limit = 20, lastMessageId) => {
        let url = `messages/groupId=${props.groupId}&limit=${limit}`;
        if(lastMessageId !== undefined)
          url = `messages/groupId=${props.groupId}&limit=${limit}/sinceId=${lastMessageId}`;
          
        
        const response = await fetch(url, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${localStorage.getItem('token')}`
              }
        });
        if(response.ok) 
        {
            const data = await response.json();
            console.log(data);
            await setMessages(messages => data.concat(messages));
            setLoading(false);
            setFirstLoad(true);
            if(data.length < limit) // set allLoaded = true if the last messages have been sent from the server
                setAllLoaded(true);
        }
        if(response.status === 204) // set allLoaded = true if there are no past messages
        {
            setAllLoaded(true);
        }
    }

    const handleScrollUp = e => {
        let element = e.target;
        let lastMessageId = messages[0].id;
        if(element.scrollTop===0){
            fetchOldMessages(20, lastMessageId);
            setLoading(true);
        }
    }

    function scrollToBottom(div) {
        div.scrollTop = div.scrollHeight;
    }

    return (
        loggedIn &&
        <div class="container content">
            <div class="row">
                <div class="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-12">
                    <div class="card">
                        <div class="card-header">Chat</div>
                        <div class="card-body height3">
                            <ul class="chat-list" id="messages" onScroll={allLoaded ? undefined : handleScrollUp} >
                                {messages.map((data, index) =>
                                    <li class={data.fromId === user.id ? "out" : "in"} key={index}>
                                        <div class="chat-img">
                                            <img alt="Avatar" src='images/no-avatar.gif' />
                                        </div>
                                        <div class="chat-body">
                                            <div class="chat-message">
                                                <h5>{data.fromId}</h5>
                                                <p>{data.content}</p>
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