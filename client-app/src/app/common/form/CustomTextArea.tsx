import { useField } from 'formik';
import React from 'react';
import { Form, Label } from 'semantic-ui-react';

interface Props {
  placeholder: string;
  name: string;
  rows: number;
  label?: string;
}

export default function CustomTextArea(props: Props) {
  const [field, meta] = useField(props.name);

  // the !! makes the meta.error field a boolean (it's either a string or undefined)
  return (
    <Form.Field error={meta.touched && !!meta.error}>
      <label>{props.label}</label>
      <textarea {...field} {...props} />
      {meta.touched && meta.error ? (
        <Label basic color='red'>
          {meta.error}
        </Label>
      ) : null}
    </Form.Field>
  );
}
