import React, { useState, useContext } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import { AccountComp } from './Account/AccountComp';
import { UserContext } from '../UserContext';
import { ProfilePage } from './ProfilePage';
import { Profile } from './NewProfile/Profile';

export const NavMenu = (props) => {
  const { loggedIn } = useContext(UserContext);
  const [collapsed, setCollapsed] = useState(true);

  const toggleNavbar = () => setCollapsed(!collapsed);

  return (
    <header>
      <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
        <Container>
          <NavbarBrand tag={Link} to="/">Karma</NavbarBrand>
          <NavbarToggler onClick={toggleNavbar} className="mr-2" />
          <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar>
            <ul className="navbar-nav flex-grow">
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
              </NavItem>
              {loggedIn && <NavItem>
                <NavLink tag={Link} className="text-dark" to="/add-listing">Add New Listing</NavLink>
                <NavLink tag={Link} className="text-dark" to="/user-listings">My Listings</NavLink>
                <NavLink className="text-dark">
                  <span className="node-link" onClick={() => props.toggleChat()}>Chat</span>
                </NavLink>
              </NavItem>}
              <NavItem>
                <AccountComp />
              </NavItem>
              {loggedIn &&<NavItem>
                <NavLink tag={Link} className="text-dark" to="/Profile" > Profile</NavLink>
              </NavItem>}
            </ul>
          </Collapse>
        </Container>
      </Navbar>
    </header>
  );
};
