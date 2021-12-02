import React, { useEffect, useState } from 'react';
import { ListingComp } from './ListingComp';
import { Pagination } from './Pagination';
import PageNotFound from './PageNotFound';

export const ListingsComp = ({ url = 'listing'}) => {
    const [loading, setLoading] = useState(true);
    const [listingsData, setListingsData] = useState([]);
    const [searchTerm, setSearchTerm] = useState('');
    const [currentPage, setCurrentPage] = useState(1);
    const [listingsPerPage] = useState(5);
    const [totalListings, setTotalListings] = useState();
    const [serverError, setServerError] = useState(false);

    const fetchData = async () => {
        const response = await fetch(url, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${localStorage.getItem('token')}`
              }
        });
        if (response.ok) {
            const data = await response.json();
            setListingsData(data);
            setTotalListings(data.length);
            setLoading(false);
            setServerError(false);
        } else if (response.status === 500) {
            setServerError(true);
        }
    };

    useEffect(() => {
        fetchData();
    }, [url]);

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

    const indexOfLastListing = currentPage * listingsPerPage;
    const indexOfFirstListing = indexOfLastListing - listingsPerPage;

    const FilterListings = () => {
        const filteredListings = listingsData.filter((val) => {
            if (searchTerm === "") {
                return val
            }
            else if (val.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
                val.location.country.toLowerCase().includes(searchTerm.toLowerCase()) ||
                val.location.city.toLowerCase().includes(searchTerm.toLowerCase())) {
                return val
            }
        });
        useEffect(() => {
            setTotalListings(filteredListings.length);
        }, []);

        const currentListings = filteredListings.slice(indexOfFirstListing, indexOfLastListing);
        return (currentListings.map(data => <li key={data.id}><ListingComp {...data} refresh={fetchData} /></li>));
    }

    const paginate = (pageNumber) => setCurrentPage(pageNumber);

    return (serverError ? < PageNotFound /> :
        <div>
            {loading ? <p><em>Loading...</em></p>
                : <><input
                    type="text"
                    placeholder="Search..."
                    onChange={(event) => {
                        setSearchTerm(event.target.value);
                    }}
                />
                    <select onChange={(e) => sortArray(e.target.value)}>
                        <option id="sortBy..">Sort by...</option>
                        <option value="dateDesc">Date DESC</option>
                        <option value="dateAsc">Date ASC</option>
                    </select>

                    <ul>
                        {listingsData != null &&
                            <FilterListings />}
                    </ul>
                    <Pagination listingsPerPage={listingsPerPage} totalListings={totalListings} paginate={paginate} /> </>
            }
        </div>
    );
}