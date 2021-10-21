import { Switch } from 'react-router-dom';
import React, { useState, useEffect } from 'react';

import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import AddListing from './components/AddListing';
import { ListingIdPage } from './components/ListingIdPage';
import PageNotFound from './components/PageNotFound';
import { UserListingsComp } from './components/UserListingsComp';
import { UserContext } from './UserContext';
import { Chat } from './components/Chat'

import './custom.css'

export default function App() {
  const [user, setUser] = useState({});
  const [loggedIn, setLoggedIn] = useState(false);

  useEffect(() => {
    const fetchUser = async () => {
      const token = localStorage.getItem('token');

      if (token) {
        const response = await fetch('user', {
          method: 'GET',
          headers: {
            'Content-type': 'application/json',
            'Authorization': `Bearer ${token}`
          }
        });
        if (response.ok) {
          const newUser = JSON.parse(await response.text());
          newUser.token = token;
          setUser(newUser);
          setLoggedIn(true);
        }
      }
    }
    fetchUser();
  }, []);

  return (
    <UserContext.Provider value={{ loggedIn, setLoggedIn, user, setUser }}>
      <Layout>   
          <Switch>
           <Route exact path='/' component={Home} />
           <Route path='/add-listing' component={AddListing} />
           <Route path='/user-listings' component={UserListingsComp} />
           <Route exact path='/details/:id' component={ListingIdPage} />
           <Route exact path='/chat' component={Chat} />
           <Route component={PageNotFound}/>
          </Switch>
      </Layout>
    </UserContext.Provider>
  );
}
