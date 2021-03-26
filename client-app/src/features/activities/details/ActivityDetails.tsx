import React, { useEffect } from 'react';
import { observer } from 'mobx-react-lite';
import { useParams } from 'react-router';
import { Button, Card, Image } from 'semantic-ui-react';
import LoadingComponent from '../../../app/layout/LoadingComponent';
import { useStore } from '../../../app/stores/store';
import { Link } from 'react-router-dom';

function ActivityDetails() {
    const {activityStore} = useStore();
    const {selectedActivity: activity, loadActivity, loadingInitial} = activityStore;
    const {id} = useParams<{id: string}>();

    useEffect(() => {
        if (id) {
            loadActivity(id);
        }
    }, [id, loadActivity]);

    if (loadingInitial || !activity) return <LoadingComponent />; //!activity part is just temp until we get error handling

    return (
        <Card fluid>
            <Image src={`/assets/categoryImages/${activity.category}.jpg`} />
            <Card.Content>
                <Card.Header>{activity.title}</Card.Header>
                <Card.Meta>
                    <span>{activity.date}</span>
                </Card.Meta>
                <Card.Description>{activity.description}</Card.Description>
                <Card.Content extra>
                    <Button.Group widths='2'>
                        <Button as={Link} to={`/manage/${activity.id}`} basic color='blue' content='Edit' />
                        <Button as={Link} to='/activities' basic color='grey' content='Cancel' />
                    </Button.Group>
                </Card.Content>
            </Card.Content>
        </Card>
    )
}

export default observer(ActivityDetails);