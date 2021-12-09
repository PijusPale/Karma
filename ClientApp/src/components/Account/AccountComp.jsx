import React, { useContext, useState } from 'react';
import { Link } from 'react-router-dom';
import { Modal, NavLink } from 'reactstrap';
import { Button } from 'reactstrap';
import { UserContext } from '../../UserContext';
import { ConfirmationButton } from '../ConfirmationButton';
import { AccountBox } from "./AccountCompLayout";

export const AccountComp = () => {
    
    const [modal, setModal] = useState(false);

    const { loggedIn, setLoggedIn, user: currentUser, setUser: setCurrentUser } = useContext(UserContext);

    const toggle = () => {
        setModal(!modal);
    };

    const onLogOut = () => {
        setLoggedIn(false);
        setCurrentUser({});
        localStorage.removeItem('token');
        setModal(false);
    }

    return (loggedIn ?
        <div>
            <NavLink tag={Link} className="text-dark" to="/profile">
                {`${currentUser.firstName} ${currentUser.lastName}`}
            </NavLink>
            <ConfirmationButton onSubmit={onLogOut} submitLabel={'Log out'}
                prompt={'Are you sure you want to log out?'}>Log out</ConfirmationButton> 
        </div> // #TODO open homepage when logging out
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
