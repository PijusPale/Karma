import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import { Modal, ModalHeader, ModalBody, ModalFooter, Input, Label, NavLink } from 'reactstrap';
import { Button } from 'reactstrap';

export const AccountComp = () => {
    const [modal, setModal] = useState(false);
    
    const [username, setUsername] = useState('');
    const [incorrectUsername, setIncorrectUsername] = useState(false);
    const [currentUser, setCurrentUser] = useState(JSON.parse(localStorage.getItem('currentUser')));
    
    const [loggedIn, setLoggedIn] = useState(!!currentUser);
    
    const toggle = () => {
        setModal(!modal);
        setIncorrectUsername(false);
    };

    const onLogIn = async () => {
        const response = await fetch('user/authenticate', {
            method: 'POST',
            headers: { 'Content-type': 'application/json' },
            body: JSON.stringify({ username }),
        });

        if (response.ok) {
            setIncorrectUsername(false);
            const user = await response.text();
            setCurrentUser(JSON.parse(user));
            setLoggedIn(true);
            localStorage.setItem('currentUser', user);
            toggle();
        }
        else {
            setIncorrectUsername(true);
        }
    };

    const onLogOut = () => {
        setLoggedIn(false);
        setCurrentUser(null);
        localStorage.removeItem('currentUser');
    }

    return (loggedIn ?
        <div>
            <NavLink tag={Link} className="text-dark" to="/profile">
                {`${currentUser.firstName} ${currentUser.lastName}`}
            </NavLink>
            <Button className="text-dark" onClick={onLogOut}>Log out</Button>
        </div>
        : <div>
            <Button className="text-dark" onClick={toggle}>Log in</Button>
            <Modal autoFocus={false} isOpen={modal} toggle={toggle}>
                <ModalHeader><Label>Log in</Label></ModalHeader>
                <ModalBody>
                    <Input autoFocus={true} onChange={e => setUsername(e.target.value)} 
                    onKeyPress={(t) => t.key === 'Enter' && onLogIn()} type="text" placeholder="Write your username" />
                    <Label hidden={!incorrectUsername}>Username not found</Label>
                </ModalBody>
                <ModalFooter>
                    <Button color="primary" onClick={onLogIn}>Log in</Button>
                    <Button color="secondary" onClick={toggle}>Cancel</Button>
                </ModalFooter>
            </Modal>
        </div>);
}
