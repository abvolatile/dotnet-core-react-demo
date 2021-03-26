import React from 'react';
import { Container } from 'semantic-ui-react';
import Navbar from './Navbar';
import ActivityDashboard from '../../features/activities/dashboard/ActivityDashboard';
import { observer } from 'mobx-react-lite';
import HomePage from '../../features/home/HomePage';
import { Route, Switch, useLocation } from 'react-router-dom';
import ActivityForm from '../../features/activities/form/ActivityForm';
import ActivityDetails from '../../features/activities/details/ActivityDetails';
import { ToastContainer } from 'react-toastify';
import NotFound from '../../features/errors/NotFound';
import TestError from '../../features/errors/testError';
import ServerError from '../../features/errors/ServerError';

function App() {
  const location = useLocation();
  
  //the <> </> is shorthand for Fragment (no need to import), fyi
  //also - that Route with path={'/(.+)'} is basically a container route to say that every path after the main '/' homepage one will have a different layout depending on what's in the render prop
  //anything that doesn't match any of the other routes will hit that NotFound route (AS LONG AS WE SURROUND OUR ROUTES WITH A SWITCH)
  return (
    <> 
      <ToastContainer position='bottom-right' hideProgressBar />
      <Route exact path='/' component={HomePage} />
      <Route
        path={'/(.+)'}
        render={() => (
          <>
            <Navbar />
            <Container style={{marginTop:'7em'}}>
              <Switch>
                <Route exact path='/activities' component={ActivityDashboard} />
                <Route path='/activities/:id' component={ActivityDetails} />
                <Route key={location.key} path={['/createActivity', '/manage/:id']} component={ActivityForm} />
                <Route path='/errors' component={TestError} />
                <Route path='/server-error' component={ServerError} />
                <Route component={NotFound} />
              </Switch>
            </Container>
          </>
        )}
      />
    </>
  );
}

export default observer(App);
