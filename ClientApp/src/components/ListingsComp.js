import React, { useEffect, useState } from 'react';
import { ListingComp } from './ListingComp';

export const ListingsComp = () => {
  // Declare a new state variable, which we'll call "count"
  const [loading, setLoading] = useState(true);
  const [listingsData, setListingsData] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      const response = await fetch('listing');
      const data = await response.json();
      setListingsData(data);
      setLoading(false);
    }
    fetchData();
  }, []);

  return (
    <div>
      {loading
        ? <p><em>Loading...</em></p>
        : <ul> {listingsData.map(data => <li key={data.id}><ListingComp {...data} /></li>)} </ul>
      }
    </div>
  );
}