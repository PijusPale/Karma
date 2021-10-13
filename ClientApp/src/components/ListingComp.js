import React from 'react';

// #TODO add defaultImage to display when image is not found or is loading
export const ListingComp = (props) =>
(<div className="row" style={{ borderStyle: "solid", borderWidth: "2px", marginTop: "10px" }}>
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
  </div>
</div>);
