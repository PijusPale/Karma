import React, { useContext, useState } from 'react';
import { Button, Label, Modal, ModalBody, ModalHeader } from 'reactstrap';
import { UserContext } from '../UserContext';
import AddListing from './AddListing';
import { ConfirmationButton } from './ConfirmationButton';

export const ListingComp = (props) => {
  const { user } = useContext(UserContext);
  const [update, setUpdate] = useState(false);

  const toggleModal = () => setUpdate(!update);

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

  return (
    <div>
      <div className="row" style={{ borderStyle: "solid", borderWidth: "2px", marginTop: "10px" }}>
        <div>
          <a href={'/details/' + props.id} target="_blank">      
            <img src={props.imagePath} alt="defaultImage" />
          </a>
          <p>Location: {props.location}</p>
        </div>
        <div>
          <h2>{props.name}</h2>
          <p>{props.description}</p>
          <p>Quantity: {props.quantity}</p>
          <p>Date: {props.datePublished.slice(0, 10)}</p>
          {user && user.id === props.ownerId &&
            <>
              <ConfirmationButton color='danger' onSubmit={onDelete} submitLabel={'Delete'}
                prompt={'Are you sure you want to delete this listing?'}>Delete</ConfirmationButton>
              <Button color="primary" onClick={() => setUpdate(true)}>Update</Button>
            </>}
        </div>
      </div>
      <Modal isOpen={update} toggle={toggleModal}>
        <ModalHeader><Label>Update</Label></ModalHeader>
        <ModalBody><AddListing {...props} update={true}
          afterSubmit={() => { toggleModal(); props.refresh(); }} /></ModalBody>
      </Modal>
    </div>);
};
