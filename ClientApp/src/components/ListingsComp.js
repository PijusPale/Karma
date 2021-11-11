import React, { useEffect, useState } from 'react';
import { ListingComp } from './ListingComp';
import PageNotFound from './PageNotFound';

export const ListingsComp = () => {
  const [loading, setLoading] = useState(true);
  const [listingsData, setListingsData] = useState([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [serverError, setServerError] = useState(false);
  
  const fetchData = async () => {
    const response = await fetch('listing');
    if (response.ok) {
        const data = await response.json();
        setListingsData(data);
        setLoading(false);
    } else if (response.status == 500) {
        setServerError(true);
    }
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
                if (document.getElementById("sortBy..") != null)
                    document.getElementById("sortBy..").remove();
                break;
            case 'dateAsc':
                setListingsData(
                    [...listingsData].sort((a, b) => {
                        return new Date(b['datePublished']) - new Date(a['datePublished'])
                    }).reverse()
                )
                if (document.getElementById("sortBy..") != null)
                    document.getElementById("sortBy..").remove();
                break;
            default:
                break;
        }
    }

  return (
      serverError ? <PageNotFound /> :
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
                      <option id="sortBy..">Sort by...</option>
                      <option value="dateDesc">Date DESC</option>
                      <option value="dateAsc">Date ASC</option>
                  </select>

                  <ul> {listingsData.filter((val) => {
                      if (searchTerm == "") {
                          return val
                      }
                      else if (val.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
                          val.location.country.toLowerCase().includes(searchTerm.toLowerCase()) ||
                          val.location.city.toLowerCase().includes(searchTerm.toLowerCase())) {
                          return val
                      }
                  }).map(data => <li key={data.id}><ListingComp {...data} refresh={fetchData} /></li>)} </ul></>
      }
    </div>
  );
}