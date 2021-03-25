import React, { useEffect } from 'react';
import { Container } from 'semantic-ui-react';
import Navbar from './Navbar';
import ActivityDashboard from '../../features/activities/dashboard/ActivityDashboard';
import LoadingComponent from './LoadingComponent';
import { useStore } from '../stores/store';
import { observer } from 'mobx-react-lite';

function App() {
  const {activityStore} = useStore();

  useEffect(() => {
    activityStore.loadActivities();
  }, [activityStore]); //this 2nd parameter (array of dependencies) ensures we don't run this useEffect in an endless loop 
    //(because it would rerender, go out and get that stuff, which rerenders, etc, etc) - this tells it to only run once 

  if (activityStore.loadingInitial) return <LoadingComponent content='Loading app...' />

  //the <> </> is shorthand for Fragment (no need to import), fyi
  return (
    <> 
      <Navbar />
      <Container style={{marginTop:'7em'}}>
        <ActivityDashboard />
      </Container>
    </>
  );
}

export default observer(App);
