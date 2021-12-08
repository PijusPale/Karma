import React, { useState, useEffect } from 'react';
import { Canvas } from '@react-three/fiber';
import { OrbitControls } from '@react-three/drei';
import { Block } from './Block';
import { gridSize } from './Plants';

const generateGridCoords = (size) => {
    let arr = [];
    const helper = Math.floor(size / 2);
    for (let i = -1 * helper; i < helper + 1; i++) {
        for (let j = -1 * helper; j < helper + 1; j++) {
            arr.push([i, -1, j]);
        }
    }
    return arr;
};
let groundCoords = generateGridCoords(gridSize);

export const GardenComp = ({ username, onPlaceChosen }) => {
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

    return (<Canvas style={{ height: "400px" }} camera={{position: [2, 3, 7]}}>
        <OrbitControls />
        <ambientLight intensity={0.5} />
        <spotLight intensity={0.3} position={[-10, 15, 10]} angle={0.3} />
        {groundCoords.map((pos, ind) =>
            <Block key={pos.join('')} position={pos} onPlaceChosen={onPlaceChosen}
                plant={garden !== null && garden[Math.floor(ind / gridSize)][ind % gridSize]} />
        )}
    </Canvas>);
}
