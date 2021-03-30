import { makeAutoObservable, reaction } from 'mobx';
import { ServerError } from '../models/serverError';

export default class CommonStore {
  error: ServerError | null = null;
  token: string | null = window.localStorage.getItem('jwt');
  appLoaded = false;

  constructor() {
    makeAutoObservable(this);

    reaction(
      () => this.token,
      (token) => {
        if (token) {
          window.localStorage.setItem('jwt', token);
        } else {
          window.localStorage.removeItem('jwt');
        }
      }
    ); //this reaction will 'watch' the value of this.token and if it changes, this function will run
  }

  setServerError = (error: ServerError) => {
    this.error = error;
  };

  setToken = (token: string | null) => {
    //if (token) window.localStorage.setItem('jwt', token); //using the reaction, we don't need this part anymore
    this.token = token;
  };

  setAppLoaded = () => {
    this.appLoaded = true;
  };
}
