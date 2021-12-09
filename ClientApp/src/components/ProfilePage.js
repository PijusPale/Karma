import React, { Suspense, useState } from 'react';
import { useParams } from 'react-router-dom';
import { GardenComp } from '../garden/garden';
import PageNotFound from './PageNotFound';

export const ProfilePage = () => {
    const { username } = useParams();
    const [pageNotFound, setPageNotFound] = useState(false);

    return (pageNotFound ? <PageNotFound /> :
    <div>
        <p>{username}</p>
        <Suspense fallback={<div>Loading... </div>}>
            <GardenComp username={username} onUserNotFound={() => setPageNotFound(true)}/>
        </Suspense>
    </div>);
}