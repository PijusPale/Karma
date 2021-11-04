import React, { useEffect, useState, useContext } from 'react';
import { UserContext } from '../UserContext';
import { ListingComp } from './ListingComp'; 

export const UserListingsComp = () => {
    const [loading, setLoading] = useState(true);
    const [listingsData, setListingsData] = useState([]);
    const [listingsType, setListingsType] = useState('posted');
    const { user, loggedIn } = useContext(UserContext);

    const fetchDataUrl = async (url) => {
        const response = await fetch(url, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${localStorage.getItem('token')}`
              }
        });
        if(response.ok) 
        {
            const data = await response.json();
            setListingsData(data);
            setLoading(false);
        }
    };

    const fetchData = () => {
        if(listingsType === 'posted')
            fetchDataUrl(`listing/userId=${user.id}`);
        if(listingsType === 'requested')
            fetchDataUrl(`listing/requesteeId=${user.id}`);
    }

    useEffect(() => {
        fetchData();
    }, [listingsType, user]);

    return (
        loggedIn &&
        <div>
            <div className="btn-group" role="group" aria-label="options">
                <input type="button" className="btn btn-secondary" value="Posted Listings" onClick={() => setListingsType('posted')} />
                <input type="button" className="btn btn-secondary" value="Requested Listings" onClick={() => setListingsType('requested')} />
            </div>
            <div>
                {loading
                    ? <p><em>Loading...</em></p>
                    : <ul> {listingsData.map(data => <li key={data.id}><ListingComp {...data} refresh={fetchData} /></li>)} </ul>
                }
            </div>
        </div>
    );
}