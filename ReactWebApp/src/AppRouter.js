import React from 'react';
import { Router, Route, browserHistory } from 'react-router';
import Index from './components/Index';
import Login from './components/Login';
import Register from './components/Register';

const AppRouter = () => (
  <Router history={browserHistory}>
    <Route path="/" component={Index}/>
    <Route path="/login" component={Login}/>
    <Route path="/register" component={Register}/>
  </Router>
);

export default AppRouter;
