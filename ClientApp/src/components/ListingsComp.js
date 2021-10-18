import React, { useEffect, useState } from 'react';
import { ListingComp } from './ListingComp';

export const ListingsComp = () => {
  // Declare a new state variable, which we'll call "count"
  const [loading, setLoading] = useState(true);
  const [listingsData, setListingsData] = useState([]);
  const [searchTerm, setSearchTerm] = useState('')

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
      <div style={{ overflow: 'hidden' }}>
      {loading
              ? <p><em>Loading...</em></p>
              : <><input
                  type="text"
                  placeholder="Search..."
                  onChange={(event) => {
                      setSearchTerm(event.target.value);
                  } }
              />
                  <ul> {listingsData.filter((val) => {
                      if (searchTerm == "") {
                          return val
                      }
                      else if (val.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
                          val.location.toLowerCase().includes(searchTerm.toLowerCase())) {
                          return val
                      }
                  }).map(data => <li key={data.id}><ListingComp {...data} /></li>)} </ul></>
      }
    </div>
  );
}