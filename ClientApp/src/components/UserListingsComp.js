import React, { useEffect, useState, useContext } from 'react';
import { UserContext } from '../UserContext';
import { ListingsComp } from './ListingsComp';

export const UserListingsComp = () => {
    const [listingsType, setListingsType] = useState('posted');
    const { user, loggedIn } = useContext(UserContext);
    const [url, setUrl] = useState(`user/listings}`);

    useEffect(() => {
        if(listingsType === 'posted')
            setUrl(`user/listings`);
        if(listingsType === 'requested')
            setUrl(`listing/requesteeId=${user.id}`);
    }, [listingsType, user]);

    return (
        loggedIn &&
        <div>
            <div className="btn-group" role="group" aria-label="options">
                <input type="button" className="btn btn-secondary" value="Posted Listings" onClick={() => setListingsType('posted')} />
                <input type="button" className="btn btn-secondary" value="Requested Listings" onClick={() => setListingsType('requested')} />
            </div>
            <ListingsComp url={url} />
        </div>
    );
}