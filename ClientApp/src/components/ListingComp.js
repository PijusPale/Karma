import React, { useContext, useState } from 'react';
import { Button, Label, Modal, ModalBody, ModalHeader } from 'reactstrap';
import { UserContext } from '../UserContext';
import AddListing from './AddListing';
import { ConfirmationButton } from './ConfirmationButton';

export const ListingComp = (props) => {
  const { user, loggedIn } = useContext(UserContext);
  const [update, setUpdate] = useState(false);
  const [show, setShow] = useState(false);
  const [requestResponse, setResponse] = useState(0);

  const toggleModal = () => setUpdate(!update);

  const CustomAlert = () => {
    let variant, description;

    if(requestResponse === 200)
    {
      variant = 'alert alert-success';
      description = 'You have successfully requested the item.';
    }
    if(requestResponse === 403)
    {
      variant = 'alert alert-danger';
      description = 'You cannot request your own item.';
    }
    if(requestResponse === 409)
    {
      variant = 'alert alert-warning';
      description = 'You have already requested this item.';
    }

    if(show) {
      return(
        <div className={variant} role="alert">
          {description}
        </div> 
      );
    }   
  }

  const onDelete = async () => {
    const res = await fetch(`listing/${props.id}`, {
      method: 'DELETE',
      headers: {
        'Content-type': 'application/json',
        'Authorization': `Bearer ${localStorage.getItem('token')}`
      }
    });
    res.ok && props.refresh();
  };

  const onRequest = async () => {
    const res = await fetch(`listing/request/${props.id}`, {
      method: 'GET',
      headers: {
        'Authorization': `Bearer ${localStorage.getItem('token')}`
      }
    });
    setResponse(res.status);  
    setShow(true);
  };

  return (
    <div>
      <div className="row" style={{ borderStyle: "solid", borderWidth: "2px", marginTop: "10px" }}>
        <div>
          <img src={props.imagePath} alt="defaultImage" />
          <p>Location: {props.location}</p>
        </div>
        <div>
          <h2>{props.name}</h2>
          <p>{props.description}</p>
          <p>Quantity: {props.quantity}</p>
          <p>Item condition: {props.condition}</p>
          <p>Date: {props.datePublished.slice(0, 10)}</p>
          {loggedIn && user.id === props.ownerId &&
            <>
              <ConfirmationButton color='danger' onSubmit={onDelete} submitLabel={'Delete'}
                prompt={'Are you sure you want to delete this listing?'}>Delete</ConfirmationButton>
              <Button color="primary" onClick={() => setUpdate(true)}>Update</Button>
            </>}
          {loggedIn && user.id !== props.ownerId && !requestResponse &&
            <> 
            {props.requestedUserIDs.includes(user.id) 
            ? <Button color="secondary" disabled>You have already requested this item</Button>
            : <Button color="secondary" onClick={onRequest}>Request Item</Button> 
            }
            </>}
          {CustomAlert()}
        </div>
      </div>
      <Modal isOpen={update} toggle={toggleModal}>
        <ModalHeader><Label>Update</Label></ModalHeader>
        <ModalBody><AddListing {...props} update={true}
          afterSubmit={() => { toggleModal(); props.refresh(); }} /></ModalBody>
      </Modal>
    </div>);
};
