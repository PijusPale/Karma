import React, { useRef, useState } from 'react';
import Tree from './tree';
import Grass from './grass';
import Flower from './flower';

export const Block = (props) => {
  // This reference gives us direct access to the THREE.Mesh object
  const group = useRef();
  const [hovered, hover] = useState(false);
  const [clicked, click] = useState(false);

  const onClick = (e) => {
    click(!clicked);
    //console.log(e.eventObject.position)
    //e.eventObject.position
  };
  return (
    <group ref={group} {...props} dispose={null}>
      <mesh
        scale={1}
        onClick={(e) => { e.stopPropagation(); onClick(e) }}
        onPointerOver={(e) => {  e.stopPropagation(); hover(true) }}
        onPointerOut={(e) => { e.stopPropagation(); hover(false) }}>
        <boxGeometry args={[1, 0.2, 1]} />
        <meshStandardMaterial color={hovered ? 'hotpink' : '#63a844'} />
      </mesh>
      <mesh position={[0,-0.5, 0]}>
        <boxGeometry args={[1, 0.8, 1]} />
        <meshStandardMaterial color='#605139' />
      </mesh>
      {((props.position[0] % 3 === 0 && props.position[2] % 3 === 0) ||
        (Math.abs(props.position[0]) % 2 === 1 && Math.abs(props.position[2]) % 2 === 1))
         && <Grass position={[0, 0.1, 0]} scale={1}/>}
      {props.plant === 'tree' && clicked && <Tree position={[0, 1, 0]}/>}
      {props.plant === 'flower' && clicked && <Flower position={[0, 0, 0]}/>}
    </group>
  );
};

/*Make text:
import { Text } from "@react-three/drei";
<Text scale={[10, 10, 10]}
        color="black" // default
        anchorX="center" // default
        anchorY="middle" // default
        >Labas</Text>
        */