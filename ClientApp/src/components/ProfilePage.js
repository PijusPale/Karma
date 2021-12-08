import React, { Suspense, useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { GardenComp } from '../garden/garden';

export const ProfilePage = () => {
    const { username } = useParams();

    return (<div>
        <p>{username}</p>
        <Suspense fallback={<div>Loading... </div>}>
            <GardenComp username={username}/>
        </Suspense>
    </div>);
}