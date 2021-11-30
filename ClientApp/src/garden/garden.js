import React, { Suspense } from 'react';
import { Canvas } from '@react-three/fiber'
import Tree from './tree'
import { OrbitControls } from '@react-three/drei'
import { Block } from './Block';

const generateGridCoords = (size) => {
    let arr = [];
    const helper = Math.floor(size/2);
    for (let i = -1*helper; i < helper+1; i++) {
        for (let j =-1*helper; j < helper + 1; j++) {
            arr.push([i, -3, j])
        }
    }
    return arr;
};
const gridSize = 6; //should be odd
let groundCoords = generateGridCoords(gridSize);

export const GardenComp = () => {

    return (<Canvas style={{ height: "400px" }}>
        <OrbitControls position0={[0, 5, -100]}/>
        <ambientLight intensity={0.5} />
        <spotLight position={[10, 15, 10]} angle={0.3} />
        <Suspense fallback={null}>
            <Tree scale={26} position={[0, -1, 0]} />
        </Suspense>
        {groundCoords.map(pos => 
            <Block position={pos} />
        )}
        {/* <Block position={[0, -3, 0]} /> */}
        {/* <Environment preset="sunset" background /> */}
        {/* <primitive object={dae.scene} dispose={null} /> */}
    </Canvas>);
}
