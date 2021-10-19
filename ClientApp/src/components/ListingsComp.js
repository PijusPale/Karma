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

  useEffect(() => {
    fetchData();
  }, []);

  return (
    <div>
      {loading
        ? <p><em>Loading...</em></p>
        : <ul> {listingsData.map(data => <li key={data.id}><ListingComp {...data} refresh={fetchData} /></li>)} </ul>
      }
    </div>
  );
}