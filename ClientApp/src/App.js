import React, { Component } from 'react';
import { BrowserRouter as Router, Switch } from 'react-router-dom';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';
import AddListing from './components/AddListing';
import { ListingIdPage } from './components/ListingIdPage';
import PageNotFound from './components/PageNotFound';

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
        <Layout>
            <Router>
                <Switch>
                    <Route exact path='/' component={Home} />
                    <Route path='/counter' component={Counter} />
                    <Route path='/fetch-data' component={FetchData} />
                    <Route path='/add-listing' component={AddListing} />
                    <Route exact path='/details/:id' component={ListingIdPage} />
                    <Route component={PageNotFound}/>
                </Switch>
            </Router>
      </Layout>
    );
  }
}
