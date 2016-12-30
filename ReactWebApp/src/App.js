import React from 'react';
import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider';
import AppRouter from './AppRouter';

const App = () => (
  <MuiThemeProvider>
    <AppRouter />
  </MuiThemeProvider>
);

export default App;
