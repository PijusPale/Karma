import React, { useEffect, useState, useContext } from 'react';
import { useParams } from 'react-router-dom';
import { UserContext } from '../UserContext';
import PageNotFound from './PageNotFound';
import { Chat } from './Chat';
import md5 from 'md5';

//TODO: display selected user's name, case when selection value is None.

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
  const [chatShow, setChatShow] = useState(false);
  const [recipientId, setrecipientId] = useState();

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
        const response = await fetch(`listing/id=${props.id}/reserve=${reserve}/for=${recipientId}`, {
          method: 'POST',
          headers: {
            'Authorization': `Bearer ${localStorage.getItem('token')}`
          },
        });

        if(response.ok) {
          setListingReservedResponse(true);
        }
      }
    }
    sendData();
  };

  const redirectToChat = () => {
    setChatShow(true);
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

    {loggedIn && user.id === props.ownerId && !listingReservedResponse && !chatShow &&
      <div align="center">
        <h5>Select user to donate the item to:</h5>
        <select class ="form-select" onChange={e => setrecipientId(e.target.value)}>
          <option selected>None</option>
          {users.map( (data, index) => <option key={index} value={data.id}>{data.firstName} {data.secondName}</option>)}
        </select>
        <br/>
        <div class="text-center">
          {(users && users.length)
            ? <button class="btn btn-outline-dark" onClick={onDonate}>Donate</button>
            : <button class="btn btn-outline-dark" disabled>Donate</button>
          }
          <br/>
        </div> 
      </div>
    }

    {loggedIn && user.id === props.recipientId && !chatShow &&
    <div align="center">
      <div class="alert alert-success" role="alert">You have received the item!<br/>Discuss further item collection with the listing owner.</div>
      <button class="btn btn-outline-dark" onClick={redirectToChat}>Chat with user</button>
    </div>
    } 
    {loggedIn && user.id === props.OwnerId && listingReservedResponse && !chatShow &&
      <div align="center">
        <div class="alert alert-success" role="alert">Listing reserved for the selected user!</div>
        <button class="btn btn-outline-dark" onClick={redirectToChat}>Chat with user</button>
      </div>
    }

    {chatShow && <Chat groupId={md5(props.OwnerId + recipientId + props.Id)}/>}
  </div>
  );
}
