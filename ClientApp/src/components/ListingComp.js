import React, { useContext } from 'react';
import { Button } from 'reactstrap';
import { UserContext } from '../UserContext';

export const ListingComp = (props) => {
  const { user } = useContext(UserContext);

  return (<div className="row" style={{ borderStyle: "solid", borderWidth: "2px", marginTop: "10px" }}>
    <div>
      <img src={props.imagePath} alt="defaultImage" />
      <p>Location: {props.location}</p>
    </div>
    <div>
      <h2>{props.name}</h2>
      <p>{props.description}</p>
      <p>Quantity: {props.quantity}</p>
      <p>Date: {props.datePublished.slice(0, 10)}</p>
      {user && user.id === props.ownerId && <Button onClick={() => props.onDelete(props.id)}>Delete</Button>}
    </div>
  </div>);
};
