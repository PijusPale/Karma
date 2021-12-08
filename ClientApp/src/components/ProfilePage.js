import React, { Suspense, useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { GardenComp } from '../garden/garden';

export const ProfilePage = () => {
    const { username } = useParams();
    const [garden, setGarden] = useState(null);

    const fetchData = async () => {
        const response = await fetch(`garden/${username}`);
        if (response.ok) {
            setGarden(await response.json());
        }
    }
    useEffect(() => {
        fetchData();
    }, []);

    return (<div>
        <p>{username}</p>
        <Suspense fallback={<div>Loading... </div>}>
            <GardenComp garden={garden}/>
        </Suspense>
    </div>);
}