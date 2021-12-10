import React from 'react';
import { Canvas } from '@react-three/fiber';

export const DisplayPlant = ({Plant}) => {    
    return (<Canvas style={{height: '150px', width: '100px' }}>
        <ambientLight intensity={0.5} />
        <spotLight position={[-3, 10, 2]} angle={0.3} intensity={0.3}/>
        <Plant position={[0, -2, 0]}/>
    </Canvas>);
}
