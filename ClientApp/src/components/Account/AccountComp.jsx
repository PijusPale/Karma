import React, { useContext, useState } from 'react';
import { Link } from 'react-router-dom';
import { Modal, ModalHeader, ModalBody, ModalFooter, Input, Label, NavLink } from 'reactstrap';
import { Button } from 'reactstrap';
import { UserContext } from '../../UserContext';
import { ConfirmationButton } from '../ConfirmationButton';
import styled from "styled-components";
import { AccountBox } from "./AccountCompLayout";

export const AccountComp = () => {
    const [modal, setModal] = useState(false);

    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [incorrectUsernameOrPassword, setincorrectUsernameOrPassword] = useState(false);

    const { loggedIn, setLoggedIn, user: currentUser, setUser: setCurrentUser } = useContext(UserContext);

    const toggle = () => {
        setModal(!modal);
        setincorrectUsernameOrPassword(false);
    };

    const onLogIn = async () => {
        const response = await fetch('user/authenticate', {
            method: 'POST',
            headers: { 'Content-type': 'application/json' },
            body: JSON.stringify({ username, password }),
        });

        if (response.ok) {
            setincorrectUsernameOrPassword(false);
            const user = JSON.parse(await response.text());
            setCurrentUser(user);
            setLoggedIn(true);
            localStorage.setItem('token', user.token);
            toggle();
        }
        else {
            setincorrectUsernameOrPassword(true);
        }
    };

    const onLogOut = () => {
        setLoggedIn(false);
        setCurrentUser({});
        localStorage.removeItem('token');
    }

    return (loggedIn ?
        <div>
            <NavLink tag={Link} className="text-dark" to="/profile">
                {`${currentUser.firstName} ${currentUser.lastName}`}
            </NavLink>
            <ConfirmationButton onSubmit={onLogOut} submitLabel={'Log out'}
                prompt={'Are you sure you want to log out?'}>Log out</ConfirmationButton>
        </div>
        : <div>
            <Button color="primary" onClick={toggle}>Log in</Button>
            <Modal className='Modal' autoFocus={false} isOpen={modal} toggle={toggle} size = "sm">
                    <AccountBox />
            </Modal>
        </div>);

        /*
        : <div>
            <Button color="primary" onClick={toggle}>Log in</Button>
            <Modal autoFocus={false} isOpen={modal} toggle={toggle}>
                <ModalHeader><Label>Log in</Label></ModalHeader>
                <ModalBody>
                    <Input autoFocus={true} onChange={e => setUsername(e.target.value)}
                        onKeyPress={(t) => t.key === 'Enter' && onLogIn()} type="text" placeholder="Write your username" />
                    <Input autoFocus={true} onChange={e => setPassword(e.target.value)}
                        onKeyPress={(t) => t.key === 'Enter' && onLogIn()} type="text" placeholder="Write your password" />
                    <Label hidden={!incorrectUsernameOrPassword}>Username and Password don't match</Label>
                </ModalBody>
                <ModalFooter>
                    <Button color="primary" onClick={onLogIn}>Log in</Button>
                    <Button color="secondary" onClick={toggle}>Cancel</Button>
                </ModalFooter>
            </Modal>
        </div>);
    }*/
}
