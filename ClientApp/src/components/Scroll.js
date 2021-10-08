import React from 'react';

const Scroll = (props) => {
  return ( 
    <div style={{overflowY: 'scroll'}}>
      {props}
    </div>	
  );
}
export default Scroll;