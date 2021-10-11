import React, { useContext, useState } from 'react';
import { Button, Label, Modal, ModalBody, ModalHeader } from 'reactstrap';
import { UserContext } from '../UserContext';
import AddListing from './AddListing';

export const ListingComp = (props) => {
  const { user } = useContext(UserContext);
  const [update, setUpdate] = useState(false);

  const toggleModal = () => setUpdate(!update);

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
          <p>Date: {props.datePublished.slice(0, 10)}</p>
          {user && user.id === props.ownerId &&
            <>
              <Button onClick={() => props.onDelete(props.id)}>Delete</Button>
              <Button onClick={() => setUpdate(true)}>Update</Button>
            </>}
        </div>
      </div>
      {update && <Modal isOpen={update} toggle={toggleModal}>
        <ModalHeader><Label>Update</Label></ModalHeader>
        <ModalBody><AddListing {...props} update={true}
          afterSubmit={() => { toggleModal(); props.refresh(); }}/></ModalBody>
      </Modal>}
    </div>);
};
