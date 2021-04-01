import React, { useEffect, useState } from 'react';
import { observer } from 'mobx-react-lite';
import { Link, useHistory, useParams } from 'react-router-dom';
import { Button, Header, Segment } from 'semantic-ui-react';
import { useStore } from '../../../app/stores/store';
import LoadingComponent from '../../../app/layout/LoadingComponent';
import { v4 as uuid } from 'uuid';
import { Formik, Form } from 'formik';
import * as Yup from 'yup';
import CustomTextInput from '../../../app/common/form/CustomTextInput';
import CustomTextArea from '../../../app/common/form/CustomTextArea';
import CustomSelect from '../../../app/common/form/CustomSelect';
import { categoryOptions } from '../../../app/common/options/categoryOptions';
import CustomDatePicker from '../../../app/common/form/CustomDatePicker';
import { ActivityFormValues } from '../../../app/models/activity';

function ActivityForm() {
  const history = useHistory();
  const { activityStore } = useStore();
  const {
    loadActivity,
    createActivity,
    updateActivity,
    loadingInitial
  } = activityStore;
  const { id } = useParams<{ id: string }>();

  const [activity, setActivity] = useState<ActivityFormValues>(
    new ActivityFormValues()
  );

  const validationSchema = Yup.object({
    title: Yup.string().required('Title is required'),
    description: Yup.string().required('Description is required'),
    category: Yup.string().required(),
    date: Yup.string().required('Date is required').nullable(),
    city: Yup.string().required('City is requred'),
    venue: Yup.string().required('Venue is required')
  });

  useEffect(() => {
    if (id) {
      loadActivity(id).then((activity) =>
        setActivity(new ActivityFormValues(activity))
      );
    }
  }, [id, loadActivity]); //we only execute the code in the effect if the dependencies have changed (prevents infinite rendering loops)

  function handleFormSubmit(activity: ActivityFormValues) {
    if (!activity.id) {
      let newActivity = {
        ...activity,
        id: uuid()
      };
      createActivity(newActivity).then(() =>
        history.push(`/activities/${newActivity.id}`)
      );
    } else {
      updateActivity(activity).then(() =>
        history.push(`/activities/${activity.id}`)
      );
    }
  }

  if (loadingInitial) return <LoadingComponent content='Loading activity...' />;

  //Formik must have initialValues and onSubmit props,
  //then it renders our form for us - passing it values (which we're naming activity), handleChange and handleSubmit (provided by Formik)
  //enableReinitialize flag allows our form to fill out with values we're editing (it's a timing thing)
  //if we use Formik's <Form> and <Field> components, the value and onChange props are AUTOMATICALLY inferred (and we don't neet to pass values or handleChange params)
  return (
    <Segment clearing>
      <Header content='Activity Details' sub color='teal' />
      <Formik
        enableReinitialize
        validationSchema={validationSchema}
        initialValues={activity}
        onSubmit={(values) => handleFormSubmit(values)}
      >
        {({ handleSubmit, isValid, isSubmitting, dirty }) => (
          <Form className='ui form' onSubmit={handleSubmit} autoComplete='off'>
            <CustomTextInput placeholder='Title' name='title' />
            <CustomTextArea
              placeholder='Description'
              name='description'
              rows={3}
            />
            <CustomSelect
              placeholder='Category'
              name='category'
              options={categoryOptions}
            />
            <CustomDatePicker
              placeholderText='Date'
              name='date'
              showTimeSelect
              timeCaption='time'
              dateFormat='MMMM d, yyyy h:mm aa'
            />
            <Header content='Location Details' sub color='teal' />
            <CustomTextInput placeholder='City' name='city' />
            <CustomTextInput placeholder='Venue' name='venue' />
            <Button
              loading={isSubmitting}
              disabled={isSubmitting || !dirty || !isValid}
              floated='right'
              positive
              type='submit'
              content='Submit'
            />
            <Button
              as={Link}
              to='/activities'
              floated='right'
              type='button'
              content='Cancel'
            />
          </Form>
        )}
      </Formik>
    </Segment>
  );
}

export default observer(ActivityForm);
