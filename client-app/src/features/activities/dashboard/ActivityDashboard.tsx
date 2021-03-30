import React, { useEffect } from 'react';
import { observer } from 'mobx-react-lite';
import { Grid } from 'semantic-ui-react';
import { useStore } from '../../../app/stores/store';
import ActivityList from './ActivityList';
import LoadingComponent from '../../../app/layout/LoadingComponent';
import ActivityFilters from './ActivityFilters';

function ActivityDashboard() {
  const { activityStore } = useStore();
  const { loadActivities, activityMap, loadingInitial } = activityStore;

  useEffect(() => {
    if (activityMap.size <= 1) loadActivities(); //only load if we haven't stored our activities in the map (<= 1 because we need to go get them if page was reloaded when creating or viewing a single activity)
  }, [activityMap.size, loadActivities]); //this 2nd parameter (array of dependencies) ensures we don't run this useEffect in an endless loop
  //(because it would rerender, go out and get that stuff, which rerenders, etc, etc) - this tells it to only run once

  if (loadingInitial)
    return <LoadingComponent content='Loading activities...' />;

  return (
    <Grid>
      <Grid.Column width='10'>
        <ActivityList />
      </Grid.Column>
      <Grid.Column width='6'>
        <ActivityFilters />
      </Grid.Column>
    </Grid>
  );
}

export default observer(ActivityDashboard);
