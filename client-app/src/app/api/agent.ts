import axios, { AxiosError, AxiosResponse } from 'axios';
import { toast } from 'react-toastify';
import { history } from '../..';
import { Activity } from '../models/activity';
import { User, UserFormValues } from '../models/user';
import { store } from '../stores/store';

const sleep = (delay: number) => {
  return new Promise((resolve) => {
    setTimeout(resolve, delay);
  });
}; //this is just to fake a longer response time

axios.defaults.baseURL = 'http://localhost:5000/api';

axios.interceptors.request.use((config) => {
  const token = store.commonStore.token;
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
}); //this makes sure we send the token with every request when we already have it!

axios.interceptors.response.use(
  async (response) => {
    await sleep(1000); //faking our response time (but the overall interceptor is a good example if you need to do something btwn req and res)
    return response;
  },
  (error: AxiosError) => {
    const { data, status, config } = error.response!; //we know we will get an error response if we get to the onReject stuff... hence the !
    switch (status) {
      case 400:
        if (typeof data === 'string') {
          toast.error(data);
        }
        if (config.method === 'get' && data.errors.hasOwnProperty('id')) {
          history.push('/not-found');
        }
        if (data.errors) {
          const modalStateErrors = [];
          for (const key in data.errors) {
            if (data.errors[key]) {
              modalStateErrors.push(data.errors[key]);
            }
          }
          throw modalStateErrors.flat(); //makes it an array of strings instead of a whole error object
        }
        break;
      case 401:
        toast.error('unauthorized');
        break;
      case 404:
        history.push('/not-found'); //this isn't technically a route, but it works!
        break;
      case 500:
        store.commonStore.setServerError(data);
        history.push('/server-error');
        break;
      default:
        break;
    }
    return Promise.reject(error);
  }
);

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

//the <T> represents a GENERIC type that will be specified later (down in the Activities obj)
const requests = {
  get: <T>(url: string) => axios.get<T>(url).then(responseBody),
  post: <T>(url: string, body: {}) =>
    axios.post<T>(url, body).then(responseBody),
  put: <T>(url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
  delete: <T>(url: string) => axios.delete<T>(url).then(responseBody)
};

const Activities = {
  list: () => requests.get<Activity[]>('/activities'),
  details: (id: string) => requests.get<Activity>(`/activities/${id}`),
  create: (activity: Activity) => requests.post<void>('/activities', activity),
  update: (activity: Activity) =>
    requests.put<void>(`/activities/${activity.id}`, activity),
  delete: (id: string) => requests.delete<void>(`/activities/${id}`)
};

const Account = {
  current: () => requests.get<User>('/account'),
  login: (user: UserFormValues) => requests.post<User>('/account/login', user),
  register: (user: UserFormValues) =>
    requests.post<User>('/account/register', user)
};

const agent = {
  Activities,
  Account
};

export default agent;
