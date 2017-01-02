import React from 'react';
import { Router, Route, IndexRoute, browserHistory } from 'react-router';
import Index from './components/Index';
import LoginScreen from './components/LoginScreen';
import ChatRoom from './components/ChatRoom';
import AdminPanel from './components/AdminPanel';
import { loggedIn } from './auth';

function requireAuth(nextState, replaceState) {
  if (!loggedIn())
    replaceState({ pathname: '/login' });
}

const AppRouter = () => (
  <Router history={browserHistory}>
    <Route path="/login" component={LoginScreen}/>
    <Route path="/" component={Index} onEnter={requireAuth}>
      <IndexRoute component={ChatRoom}/>
      <Route path="admin" component={AdminPanel}/>
    </Route>
  </Router>
);

export default AppRouter;
