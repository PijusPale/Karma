import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import PageNotFound from './PageNotFound';

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
          : <ListingPageLayout {...Data}/>
        }
      </div>
    );
}

export const ListingPageLayout = (props) =>
(<div>
    <div>
        <img src={props.imagePath} alt="defaultImage" width="500" />
        <h2>{props.name}</h2>
    </div>
    <div>
        <p>{props.location.country}, {props.location.city}, {props.location.radiusKM}km</p>
        <p>{props.description}</p>
        <p>Quantity: {props.quantity}</p>
    </div>
</div>);

