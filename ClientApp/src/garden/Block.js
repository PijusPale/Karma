import React, { useRef, useState } from 'react';

export const Block = (props) => {
    // This reference gives us direct access to the THREE.Mesh object
    const ref = useRef()
    // Hold state for hovered and clicked events
    const [hovered, hover] = useState(false)
    const [clicked, click] = useState(false)
    // Subscribe this component to the render-loop, rotate the mesh every frame
    // Return the view, these are regular Threejs elements expressed in JSX
    return (
      <mesh
        {...props}
        ref={ref}
        scale={clicked ? 1.5 : 1}
        onClick={(e) => {e.stopPropagation(); click(!clicked)}}
        onPointerOver={(e) => {e.stopPropagation(); hover(true)}}
        onPointerOut={(e) =>{e.stopPropagation(); hover(false)}}>
        <boxGeometry args={[1, 1, 1]} />
        <meshStandardMaterial color={hovered ? 'hotpink' : '#567d46'} />
      </mesh>
    )
  };