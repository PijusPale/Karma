import React, { useEffect, useState, useContext } from 'react';
import { useParams } from 'react-router-dom';
import { UserContext } from '../UserContext';
import PageNotFound from './PageNotFound';
import { Chat } from './Chat';

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
  const [recipientUser, setRecipientUser] = useState([]);
  const [listingReservedResponse, setListingReservedResponse] = useState(false);
  const [chatShow, setChatShow] = useState(false);
  const [selectedRecipientId, setSelectedRecipientId] = useState();

  useEffect(() => {
    if(props.status === 'Reserved')
      setListingReservedResponse(true);
       
    const fetchData = async () => {
      if (loggedIn && (user.id === props.userId)) {
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

  useEffect(() => {
    const getRecipientUser = () => {
      if(users){
        const data = users.find(({ id }) => id === props.recipientId);
        setRecipientUser(data);
      }
    }

    getRecipientUser();
  }, [users, recipientUser]);

  const onDonate = async () => {
    const sendData = async () => {
      var reserve = true;
      if(loggedIn && (user.id === props.userId)) {
        const response = await fetch(`listing/id=${props.id}/reserve=${reserve}/for=${selectedRecipientId}`, {
          method: 'POST',
          headers: {
            'Authorization': `Bearer ${localStorage.getItem('token')}`
          },
        });

        if(response.ok) {
          window.location.reload(false);
        }
      }
    }
    sendData();
  };

  const giveListing = async () => {
    const sendData = async () => {
      if(loggedIn && (user.id === props.userId)) {
        const response = await fetch(`listing/id=${props.id}/conclude`, {
          method: 'PUT',
          headers: {
            'Authorization': `Bearer ${localStorage.getItem('token')}`
          },
        });

        if(response.ok) {
          window.location.reload(false);
        }
      }
    }
    sendData();
  }

  const cancelReservation = async () => {
    const sendData = async () => {
      if(loggedIn && (user.id === props.userId)) {
        const response = await fetch(`listing/id=${props.id}/cancelReservation`, {
          method: 'PUT',
          headers: {
            'Authorization': `Bearer ${localStorage.getItem('token')}`
          },
        });

        if(response.ok) {
          window.location.reload(false);
        }
      }
    }
    sendData();
  }

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

    {loggedIn && user.id === props.userId && props.status === 'Created' && !chatShow &&
      <div align="center">
        <h5>Select user to donate the item to:</h5>
        <select className="form-select" onChange={e => setSelectedRecipientId(e.target.value)}>
          <option selected>None</option>
          {users.map( (data, index) => <option key={index} value={data.id}>{data.firstName} {data.secondName}</option>)}
        </select>
        <br/>
        <div className="text-center">
          {(users && users.length)
            ? <button class="btn btn-outline-dark" onClick={onDonate}>Donate</button>
            : <button class="btn btn-outline-dark" disabled>Donate</button>
          }
          <br/>
        </div> 
      </div>
    }

    {loggedIn && user.id === props.recipientId && props.status === 'Reserved' && !chatShow &&
    <div align="center">
      <div className="alert alert-success" role="alert">You have received the item!<br/>Discuss further item collection with the listing owner.</div>
      <button className="btn btn-outline-dark" onClick={redirectToChat}>Chat with user</button>
    </div>
    } 
    {loggedIn && user.id === props.userId && props.status === 'Reserved' && !chatShow && recipientUser &&
      <div align="center">
        <div className="alert alert-success" role="alert">Listing reserved for the selected user!</div>
        <button className="btn btn-outline-dark" onClick={redirectToChat}>Chat with {recipientUser.firstName}</button>
      </div>
    }

    {loggedIn && user.id === props.userId && listingReservedResponse && props.status === 'Reserved' && recipientUser &&
    <div align="center">
      <button className="btn btn-outline-dark" onClick={giveListing}>Give listing to {recipientUser.firstName}</button>
      <button className="btn btn-outline-danger" onClick={cancelReservation}>Cancel Reservation</button>
    </div>
    }

    {chatShow && <Chat/>}
  </div>
  );
}
