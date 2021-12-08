import React, { useRef, useState, useEffect } from 'react';
import Grass from './grass';
import Plants, { gridSize } from './Plants';
import { Text } from "@react-three/drei";
import GrowingPlant from './GrowingPlant';

export const Block = (props) => {
  // This reference gives us direct access to the THREE.Mesh object
  const group = useRef();
  const [hoveredGround, hoverGround] = useState(false);
  const [hoveredPlant, hoverPlant] = useState(false);
  const listingId = props.plant && props.plant.split('/')[0];
  const plantName = props.plant && props.plant.split('/')[1];
  const Plant = props.plant && Plants[plantName.replace('growing', '')];
  const [listing, setListing] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      if (listingId) {
        const response = await fetch('listing/' + listingId);
        if (response.ok) {
          const data = await response.json();
          setListing(data);
        }
      }
    }
    fetchData();
  }, [listingId]);


  const OnGroundClick = () => {
    props.onPlaceChosen && props.onPlaceChosen(props.position[0] + Math.floor(gridSize / 2), props.position[2] + Math.floor(gridSize / 2));
  };
  return (
    <group ref={group} {...props} dispose={null}>
      <mesh
        scale={1}
        onClick={(e) => { e.stopPropagation(); OnGroundClick() }}
        onPointerOver={(e) => { e.stopPropagation(); hoverGround(true) }}
        onPointerOut={(e) => { e.stopPropagation(); hoverGround(false) }}>
        <boxGeometry args={[1, 0.2, 1]} />
        <meshStandardMaterial color={(props.onPlaceChosen && hoveredGround) ? 'hotpink' : '#63a844'} />
      </mesh>
      <mesh position={[0, -0.5, 0]}>
        <boxGeometry args={[1, 0.8, 1]} />
        <meshStandardMaterial color='#605139' />
      </mesh>
      {((props.position[0] % 3 === 0 && props.position[2] % 3 === 0) ||
        (Math.abs(props.position[0]) % 2 === 1 && Math.abs(props.position[2]) % 2 === 1))
        && <Grass position={[0, 0.1, 0]} scale={1} />}
      {props.plant && (plantName.startsWith('growing') ?
        <GrowingPlant onPointerOver={(e) => { e.stopPropagation(); hoverPlant(true) }}
        onPointerOut={(e) => { e.stopPropagation(); hoverPlant(false) }} />
        : <Plant onPointerOver={(e) => { e.stopPropagation(); hoverPlant(true) }}
          onPointerOut={(e) => { e.stopPropagation(); hoverPlant(false) }} />)}
        {listing && hoveredPlant && <Text scale={[10, 10, 10]} position={[0, 4, 0]}
          color="black" // default
          anchorX="center" // default
          anchorY="middle" // default
        >{listing.name}</Text>}
    </group>
  );
};