import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Container } from 'semantic-ui-react';
import { Activity } from '../models/activity';
import Navbar from './Navbar';
import ActivityDashboard from '../../features/activities/dashboard/ActivityDashboard';
import {v4 as uuid} from 'uuid';

function App() {
  //make sure we use our Activity interface so our request is totally securely typed!
  const [activities, setActivities] = useState<Activity[]>([]);
  const [selectedActivity, setSelectedActivity] = useState<Activity|undefined>(undefined);
  const [editMode, setEditMode] = useState(false);

  useEffect(() => {
    axios.get<Activity[]>('http://localhost:5000/api/activities').then(response => {
      setActivities(response.data);
    })
  }, []); //this 2nd parameter ensures we don't run this useEffect in an endless loop (because it would rerender, go out and get that stuff, which rerenders, etc, etc) - this tells it to only run once

  function handleSelectActivity(id: string) {
    setSelectedActivity(activities.find(a => a.id === id));
  }

  function handleCancelSelectActivity() {
    setSelectedActivity(undefined);
  }

  function handleFormOpen(id?: string) {
    id ? handleSelectActivity(id) : handleCancelSelectActivity();
    //sets the selectedActivity to be undefined either way
    setEditMode(true);
  }

  function handleFormClose() {
    setEditMode(false);
  }

  function handleCreateOrEditActivity(activity: Activity) {
    activity.id ? 
      setActivities([...activities.filter(a => a.id !== activity.id), activity]) 
    : 
      setActivities([...activities, {...activity, id: uuid()}]);

    setEditMode(false);
    setSelectedActivity(activity);
  }

  function handleDeleteActivity(id: string) {
    setActivities([...activities.filter(a => a.id !== id)]);
  }

  return (
    <> 
    {/* the <> </> is shorthand for Fragment! */}
      <Navbar openForm={handleFormOpen} />
      <Container style={{marginTop:'7em'}}>
        <ActivityDashboard 
          activities={activities} 
          selectedActivity={selectedActivity} 
          selectActivity={handleSelectActivity}
          cancelSelectActivity={handleCancelSelectActivity}
          editMode={editMode}
          openForm={handleFormOpen}
          closeForm={handleFormClose}
          createOrEdit={handleCreateOrEditActivity}
          deleteActivity={handleDeleteActivity}
        />
      </Container>
    </>
  );
}

export default App;
