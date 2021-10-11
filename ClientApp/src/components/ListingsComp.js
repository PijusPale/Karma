import React, { useEffect, useState } from 'react';
import { ListingComp } from './ListingComp';

export const ListingsComp = () => {
  const [loading, setLoading] = useState(true);
  const [listingsData, setListingsData] = useState([]);
  
  const fetchData = async () => {
    const response = await fetch('listing');
    const data = await response.json();
    setListingsData(data);
    setLoading(false);
  };

  const onDelete = async (listingId) => {
    const res = await fetch(`listing/${listingId}`, {
      method: 'DELETE',
      headers: {
        'Content-type': 'application/json',
        'Authorization': `Bearer ${localStorage.getItem('token')}`
      }
    });
    res.ok && fetchData();
  };

  useEffect(() => {
    fetchData();
  }, []);

  return (
    <div>
      {loading
        ? <p><em>Loading...</em></p>
        : <ul> {listingsData.map(data => <li key={data.id}><ListingComp {...data} onDelete={onDelete} refresh={fetchData} /></li>)} </ul>
      }
    </div>
  );
}