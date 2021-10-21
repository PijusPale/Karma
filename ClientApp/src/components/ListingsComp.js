import React, { useEffect, useState } from 'react';
import { ListingComp } from './ListingComp';

export const ListingsComp = () => {
  const [loading, setLoading] = useState(true);
  const [listingsData, setListingsData] = useState([]);
  const [searchTerm, setSearchTerm] = useState('');
  
  const fetchData = async () => {
    const response = await fetch('listing');
    const data = await response.json();
    setListingsData(data);
    setLoading(false);
  };
  useEffect(() => {
    fetchData();
  }, []);

    const sortArray = type => {
        switch (type) {
            case 'dateDesc':
                setListingsData(
                    [...listingsData].sort((a, b) => {
                        return new Date(b['datePublished']) - new Date(a['datePublished'])
                    })
                )
                break;
            case 'dateAsc':
                setListingsData(
                    [...listingsData].sort((a, b) => {
                        return new Date(b['datePublished']) - new Date(a['datePublished'])
                    }).reverse()
                )
                break;
            default:
                break;
        }
    }

  return (
      <div style={{ overflow: 'hidden' }}>
      {loading ? <p><em>Loading...</em></p>
              : <><input
                  type="text"
                  placeholder="Search..."
                  onChange={(event) => {
                      setSearchTerm(event.target.value);
                  } }
              />
                  <select onChange={(e) => sortArray(e.target.value)}>
                      <option>Sort by...</option>
                      <option value="dateDesc">Date DESC</option>
                      <option value="dateAsc">Date ASC</option>
                  </select>

                  <ul> {listingsData.filter((val) => {
                      if (searchTerm == "") {
                          return val
                      }
                      else if (val.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
                          val.location.country.toLowerCase().includes(searchTerm.toLowerCase())) {
                          return val
                      }
                  }).map(data => <li key={data.id}><ListingComp {...data} /></li>)} </ul></>
      }
    </div>
  );
}