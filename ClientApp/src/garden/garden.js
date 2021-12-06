import React, { Suspense } from 'react';
import { Canvas } from '@react-three/fiber'
import Tree from './tree'
import { OrbitControls } from '@react-three/drei'
import { Block } from './Block';
import Flower from './flower';

const generateGridCoords = (size) => {
    let arr = [];
    const helper = Math.floor(size/2);
    for (let i = -1*helper; i < helper+1; i++) {
        for (let j =-1*helper; j < helper + 1; j++) {
            arr.push([i, -1, j])
        }
    }
    return arr;
};
const gridSize = 9; //should be odd
let groundCoords = generateGridCoords(gridSize);

export const GardenComp = () => {

    return (<Canvas style={{ height: "400px" }}>
        <OrbitControls/>
        <ambientLight intensity={0.5} />
        <spotLight intensity={0.3} position={[-10, 15, 10]} angle={0.3} />
        {groundCoords.map(pos => 
            <Block position={pos} plant='tree' />
        )}
        <Flower position={[0, -1, 0]}/>
    </Canvas>);
}
