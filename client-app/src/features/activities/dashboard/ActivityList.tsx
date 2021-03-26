import React, { Fragment } from 'react';
import { observer } from 'mobx-react-lite';
import { Header } from 'semantic-ui-react';
import { useStore } from '../../../app/stores/store';
import ActivityListItem from './ActivityListItem';


//another way to do it (rather than declare the function separately then exporting default after)
export default observer(function ActivityList() {
    const {activityStore} = useStore();
    const {groupedActivities} = activityStore;

    //the reason we need to use the full version of Fragment is because the shorthand one can't accept a key prop
    return (
        <>
            {groupedActivities.map(([group, activities]) => (
                <Fragment key={group}>
                    <Header sub color='teal'>
                        {group}
                    </Header>
                    {activities.map(activity => (
                        <ActivityListItem key={activity.id} activity={activity} />
                    ))}
                </Fragment>
            ))}
        </>
    )
});