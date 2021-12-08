import React, { useEffect, useState, useContext, useRef } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { UserContext } from '../UserContext';
import styles from '@chatscope/chat-ui-kit-styles/dist/default/styles.min.css'; // do not remove this, it is used
import { debounce } from '@material-ui/core';
import { MessageSeparator, ChatContainer, MessageList, Message, MessageInput, Sidebar, Conversation, ConversationList, Avatar, ConversationHeader, InfoButton, TypingIndicator } from '@chatscope/chat-ui-kit-react';
import '../chat.css';
import './chat.css';

export const Chat = (props) => {
    const inputRef = useRef();
    const [message, setMessage] = useState('');
    const [messages, setMessages] = useState([]); //array of objects with message.type, message.nick, message.text
    const [conversations, setConversations] = useState([]); //array of objects with conversations fetched from server
    const [conversation, setConversation] = useState([]); //current conversation object
    const [hubConnection, setHubConnection] = useState();
    const [loading, setLoading] = useState(true); // Loading bar while waiting for hub connection and initial fetch
    const [loadingMessages, setLoadingMessages] = useState(false);
    const [allLoaded, setAllLoaded] = useState(false);
    const [isTypingOut, setIsTypingOut] = useState(false);
    const [isTypingIn, setIsTypingIn] = useState(false);
    const [lastActive, setLastActive] = useState('');
    const { user, loggedIn } = useContext(UserContext);

    useEffect(() => {
        if (loggedIn) {
            setLoading(true);
            const connection = new HubConnectionBuilder()
                .withUrl("/ChatHub", { accessTokenFactory: () => localStorage.getItem('token') })
                .build();

            connection.on("ReceiveGroupMessage", (nick, receivedMessage) => receiveMessage(nick, receivedMessage));
            connection.on("ReceiveTypingNotification", (fromId, isTyping) => handleIsTypingIn(fromId, isTyping));
            connection.on("GetLastActive", (date) => setLastActive(timeSince(date)));

            connection.logging = true;
            connection
                .start()
                .then(() => console.log('Connection established.'))
                .catch(err => console.log(`Error while establishing connection: ${err}`));

            setHubConnection(connection);
            fetchConversations();
            setLoading(false);
        }
    }, [loggedIn]);

    useEffect(() => {
        if (hubConnection !== undefined && Object.keys(conversation).length)
            hubConnection.invoke('SendTypingNotificationAsync', conversation.groupId, user.id, isTypingOut);
    }, [isTypingOut]);

    useEffect(() => {
        handleIsTypingOut();
    }, [message]);

    const sendMessage = async () => {
        if (message !== '') {
            await hubConnection
                .invoke('SendGroupMessageAsync', conversation.groupId, user.id, message)
                .catch(err => console.error(`Send message ${err}`));

            setMessage('');
            inputRef.current.focus();
            //scrollToBottom(messagesDiv);
        }
    }

    const receiveMessage = (nick, receivedMessage) => {
        var tempMessage = {
            fromId: nick,
            content: receivedMessage,
            type: user.firstName === nick ? "out" : "in",
            dateSent: new Date().toISOString(),
            status: 'Delivered'
        };

        setMessages(messages => messages.concat(tempMessage));
        //scrollToBottom(messagesDiv);
    }

    const fetchOldMessages = async (limit = 20, lastMessageId) => {
        let url = `message/groupId=${conversation.groupId}/limit=${limit}`;
        if (lastMessageId !== undefined)
            url = `message/groupId=${conversation.groupId}/limit=${limit}/lastMessageId=${lastMessageId}`;


        const response = await fetch(url, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${localStorage.getItem('token')}`
            }
        });
        if (response.status === 200) {
            const data = await response.json();
            await setMessages(messages => data.concat(messages));
            if (data.length < limit) // set allLoaded = true if the last messages have been sent from the server
                setAllLoaded(true);
        }
        if (response.status === 204) // set allLoaded = true if there are no past messages
        {
            setAllLoaded(true);
        }
        setLoadingMessages(false);
    }

    const fetchConversations = async () => {
        const response = await fetch(`message/conversations`, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${localStorage.getItem('token')}`
            }
        });
        if (response.ok) {
            const data = await response.json();
            setConversations(data);
        }
    }

    const handleScrollUp = () => {
        if (loadingMessages) {
            return;
        }
        if (!allLoaded) {
            setLoadingMessages(true);
            let lastMessageId = messages[0].id;
            fetchOldMessages(20, lastMessageId);
        }
    }

    useEffect(() => {
        if (Object.keys(conversation).length && hubConnection) {
            hubConnection.invoke("AddToGroupAsync", conversation.groupId)
                .catch(err => console.log(`Add to group ${err}`));
            fetchOldMessages(20);
            requestLastActive(conversation.userOneId === user.id ? conversation.userTwoId : conversation.userOneId);
        }
    }, [conversation]);

    const handleIsTypingOut = () => {
        setIsTypingOut(true);
        const handleIsTypingOutDebounce = debounce(() => {
            setIsTypingOut(false);
        }, 5000);
        handleIsTypingOutDebounce();
    }

    const handleIsTypingIn = (fromId, isTyping) => {
        if (fromId !== user.id)
            setIsTypingIn(isTyping);
    }

    const onSelectConversation = (props) => {
        setConversation(props);
    }

    const requestLastActive = (userId) => {
        if (userId && hubConnection)
            hubConnection.invoke("LastActiveRequest", userId);
    }

    const handleBackButton = async () => {
        if (hubConnection)
            await hubConnection.invoke("RemoveFromGroupAsync", conversation.groupId);
        setConversation([]);
        setMessages([]);
        setMessage('');
        setIsTypingIn(false);
        setIsTypingOut(false);
        setAllLoaded(false);
        setLastActive([]);
        fetchConversations();
    }

    function timeSince(date) { //UTC format
        const rtf = new Intl.RelativeTimeFormat("en", {
            localeMatcher: "best fit",
            numeric: "always",
            style: "long",
        });
        var seconds = Math.floor((new Date() - new Date(date + "Z")) / 1000);

        var interval = seconds / 31536000;

        if (interval > 1) {
            return rtf.format(-Math.floor(interval), 'year');
        }
        interval = seconds / 2592000;
        if (interval > 1) {
            return rtf.format(-Math.floor(interval), 'month');
        }
        interval = seconds / 86400;
        if (interval > 1) {
            return rtf.format(-Math.floor(interval), 'day');
        }
        interval = seconds / 3600;
        if (interval > 1) {
            return rtf.format(-Math.floor(interval), 'hour');
        }
        interval = seconds / 60;
        if (interval > 1) {
            return rtf.format(-Math.floor(interval), 'minute');
        }
        return '';
    }

    return (
        loggedIn &&
        <div className="fixed-container border">
            {!Object.keys(conversation).length ?
                <div>
                    <Sidebar position="left" scrollable={true}>
                        <ConversationList loading={loading}>
                            {conversations.map((data, index) =>
                                data.lastSender === ""
                                    ? <Conversation key={index} name={data.listingName} info={<i>Send a message to start the conversation</i>} onClick={() => onSelectConversation(data)}>
                                        <Avatar src={data.listingImg} name={data.listingName} />
                                    </Conversation>
                                    : <Conversation key={index} name={data.listingName} lastSenderName={data.lastSender} info={data.lastMessage} onClick={() => onSelectConversation(data)}>
                                        <Avatar src={data.listingImg} name={data.listingName} />
                                    </Conversation>
                            )}
                        </ConversationList>
                    </Sidebar>
                </div>
                :
                <ChatContainer>
                    <ConversationHeader>
                        <ConversationHeader.Back onClick={handleBackButton} />
                        <Avatar src={conversation.listingImg} name={conversation.listingName} />
                        <ConversationHeader.Content userName={conversation.userOneId === user.id ? conversation.userTwoName : conversation.userOneName} info={`Active ${lastActive}`} />
                        <ConversationHeader.Actions>
                            <a href={'/details/' + conversation.listingId} target="_blank" rel="noopener noreferrer" >
                                <InfoButton title="Open Listing" />
                            </a>
                        </ConversationHeader.Actions>
                    </ConversationHeader>
                    <MessageList loadingMore={loadingMessages} typingIndicator={isTypingIn ? <TypingIndicator content={`${conversation.userOneId === user.id ? conversation.userTwoName : conversation.userOneName} is typing..`} /> : undefined} onYReachStart={handleScrollUp}>
                        {messages.reduce((acc, data, index) => {
                            if (data === undefined) {
                                return acc;
                            }
                            let nextAcc = acc;
                            let dateOlder = new Date(data.dateSent + "Z");
                            let options = { timeStyle: "short", dateStyle: "short" };

                            if (index === 0){
                                nextAcc = nextAcc.concat(
                                    <MessageSeparator key={"separator"+index} content={dateOlder.toLocaleString("en-GB", options)} />
                                )
                            }

                            nextAcc = nextAcc.concat(
                                <Message
                                    key={index}
                                    model={{
                                        message: data.content,
                                        sentTime: data.dateSent,
                                        sender:
                                            data.fromId === conversation.userOneId
                                                ? conversation.userOneName
                                                : conversation.userTwoName,
                                        direction: data.fromId === user.id ? "outgoing" : "incoming",
                                        position: "single",
                                    }}
                                />
                            );

                            if (messages[index + 1] !== undefined) {
                                let dateNewer = new Date(messages[index + 1].dateSent + "Z");
                                if (dateNewer.getTime() - dateOlder.getTime() >= 1200000) {
                                    return nextAcc.concat(
                                        <MessageSeparator key={"separator"+index} content={dateNewer.toLocaleString("en-GB", options)} />
                                    );
                                }
                            }

                            return nextAcc;
                        }, [])}
                    </MessageList>
                    <MessageInput placeholder="Type message" attachButton={false} autoFocus={true} value={message} onChange={setMessage} onSend={sendMessage} ref={inputRef} />
                </ChatContainer>
            }
        </div>
    );
};