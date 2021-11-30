import React, {Suspense} from 'react';
import { GardenComp } from '../garden/garden';
import { ListingsComp } from './ListingsComp';

export const Home = () =>(
  <Suspense fallback={<div>Loading... </div>}>
    <GardenComp />
</Suspense>
)   //<div><ListingsComp /></div>;