import React, { useEffect, useState } from 'react';

export const ListingIdPage = () => {

    const [loading, setLoading] = useState(true);
    const [Data, setData] = useState([]);
  
    useEffect(() => {
      const fetchData = async () => {
        const response = await fetch('listing/2522'); // #TODO add proper id
        const data = await response.json();
        setData(data);
        setLoading(false);
      }
      fetchData();
    }, []);
  
    return (
      <div>
        {loading
          ? <p><em>Loading...</em></p>
          : PageLayout (Data) 
        }
      </div>
    );
  }

export const PageLayout = (props) =>
(<div>
    <div>
        <img src={props.imagePath} alt="defaultImage" width="500"/>
        <h2>{props.name}</h2>
    </div>
    <div>
        <p>Location: {props.location}</p>
        <p>{props.description}</p>
        <p>Quantity: {props.quantity}</p>
    </div>
</div>);
