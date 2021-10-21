import React, { useEffect, useState, useContext } from 'react';
import { useParams } from 'react-router-dom';
import { UserContext } from '../UserContext';
import PageNotFound from './PageNotFound';

//TODO: display selected user's name.

export const ListingIdPage = () => {

  const [loading, setLoading] = useState(true);
  const [Data, setData] = useState([]);
  const { id } = useParams();
  const [notFound, setNotFound] = useState(true);

  useEffect(() => {
    const fetchData = async () => {
      const response = await fetch('listing/' + id);
      if (response.ok) {
        const data = await response.json();
        setData(data);
        setLoading(false);
        setNotFound(false);
      }
    }
    fetchData();
  }, []);

  if (notFound)
    return <PageNotFound />;


  return (
    <div>
      {loading
        ? <p><em>Loading...</em></p>
        : <ListingPageLayout {...Data} />
      }
    </div>
  );
}

export const ListingPageLayout = (props) => {
  const { user, loggedIn } = useContext(UserContext);
  const [users, setUsers] = useState([]);
  const [listingReservedResponse, setListingReservedResponse] = useState(false);

  useEffect(() => {
    if(props.isReserved)
      setListingReservedResponse(true);
    const fetchData = async () => {
      if (loggedIn && (user.id === props.ownerId)) {
        const response = await fetch(`user/getByListingId=${props.id}`, {
          method: 'GET',
          headers: {
            'Authorization': `Bearer ${localStorage.getItem('token')}`
          }
        });

        if (response.ok) {
          const data = await response.json();
          setUsers(data);
        }
      }
    }
    fetchData();
  }, [loggedIn]);

  const onDonate = async () => {
    const sendData = async () => {
      var reserve = true;
      if(loggedIn && (user.id === props.ownerId)) {
        const response = await fetch(`listing/id=${props.id}/reserve=${reserve}`, {
          method: 'POST',
          headers: {
            'Authorization': `Bearer ${localStorage.getItem('token')}`
          }
        });

        if(response.ok) {
          setListingReservedResponse(true);
        }
      }
    }
    sendData();
  };

  return (<div>
    <div>
      <img src={props.imagePath} alt="defaultImage" width="500" />
      <h2>{props.name}</h2>
    </div>
    <div>
      <p>{props.location.country}, {props.location.city}, {props.location.radiusKM}km</p>
      <p>{props.description}</p>
      <p>Quantity: {props.quantity}</p>
    </div>
    {loggedIn && user.id === props.ownerId && !listingReservedResponse && 
      <div align="center">
        <h4>Select user to donate the item to:</h4>
        <select class ="form-select">
          <option selected>None</option>
          {users.map( (data, index) => <option key={index}>{data.firstName} {data.secondName}</option>)}
        </select>
        <br/>
        <div class="text-center">
          {(users && users.length)
            ? <button align="center" class="btn btn-outline-dark" onClick={onDonate}>Donate</button>
            : <button align="center" class="btn btn-outline-dark" disabled>Donate</button>
          }
          <br/>
        </div> 
      </div>
    }
    {listingReservedResponse &&
      <div class="alert alert-success" role="alert">Listing reserved for the selected user!</div>}
  </div>
  );
}
