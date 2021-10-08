import React, { useState } from 'react';
import Scroll from './Scroll';
import { ListingsComp } from './ListingsComp';

function Search({ details }) {

  const [searchField, setSearchField] = useState("");

  const filteredListings = details.filter(
    listing => {
      return (
        listing
        .name
        .toLowerCase()
        .includes(searchField.toLowerCase())
      );
    }
  );

  const handleChange = e => {
    setSearchField(e.target.value);
  };

  function searchList() {
    return (
      <Scroll>
        <ListingsComp filteredListings={filteredListings} />
      </Scroll>
    );
  }

  return (
    <section>
      <div>
        <input 
          type = "search" 
          placeholder = "Search Products" 
          onChange = {handleChange}
        />
      </div>
      {searchList()}
    </section>
  );
}
export default Search;