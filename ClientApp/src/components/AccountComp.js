import React, { useContext, useState } from 'react';
import { Link } from 'react-router-dom';
import { Modal, ModalHeader, ModalBody, ModalFooter, Input, Label, NavLink } from 'reactstrap';
import { Button } from 'reactstrap';
import { UserContext } from '../UserContext';

export const AccountComp = () => {
    const [modal, setModal] = useState(false);
    
    const [username, setUsername] = useState('');
    const [incorrectUsername, setIncorrectUsername] = useState(false);

    const { loggedIn, setLoggedIn, user: currentUser, setUser: setCurrentUser } = useContext(UserContext);
    
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
            const user = JSON.parse(await response.text());
            setCurrentUser(user);
            setLoggedIn(true);
            localStorage.setItem('token', user.token);
            toggle();
        }
        else {
            setIncorrectUsername(true);
        }
    };

    const onLogOut = () => {
        setLoggedIn(false);
        setCurrentUser(null);
        localStorage.removeItem('token');
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
