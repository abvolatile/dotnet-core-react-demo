import { useField } from 'formik';
import React from 'react';
import { Form, Label } from 'semantic-ui-react';
import DatePicker, { ReactDatePickerProps } from 'react-datepicker';

//ReactDatePickerProps props are mostly optional, but we can make them ALL optional by using a Partial type!
export default function CustomDatePicker(props: Partial<ReactDatePickerProps>) {
    const [field, meta, helpers] = useField(props.name!); //we will make sure there's a name prop

    // the !! makes the meta.error field a boolean (it's either a string or undefined)
    //datepicker uses the js Date object (we'll have to do some converting to mesh with our BE)
    return (
        <Form.Field error={meta.touched && !!meta.error}>
            <DatePicker 
                {...field}
                {...props}
                selected={(field.value && new Date(field.value)) || null}
                onChange={value => helpers.setValue(value)}
            />
            {meta.touched && meta.error ? (
                <Label basic color='red'>{meta.error}</Label>
            ) : null}
        </Form.Field>
    )
}