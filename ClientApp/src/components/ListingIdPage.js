import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import PageNotFound from './PageNotFound';

export const ListingIdPage = () => {

    const [loading, setLoading] = useState(true);
    const [Data, setData] = useState([]);
    const { id } = useParams();
    const [Response, setResponse] = useState(false);

    useEffect(() => {
        const fetchData = async () => {
            const response = await fetch('listing/' + id);
            const data = await response.json();
            setData(data);
            setLoading(false);
            setResponse(true);
        }
      fetchData();
    }, [id]);

    if (!Response)
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
        <p>Location: {props.location}</p>
        <p>{props.description}</p>
        <p>Quantity: {props.quantity}</p>
    </div>
</div>);

