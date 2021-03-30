import { makeAutoObservable, runInAction } from 'mobx';
import { history } from '../..';
import agent from '../api/agent';
import { User, UserFormValues } from '../models/user';
import { store } from './store';

export default class UserStore {
  user: User | null = null;

  constructor() {
    makeAutoObservable(this);
  }

  get isLoggedIn() {
    return !!this.user; // the !! casts the value of this.user to a bool (if null - it's false, if not - it's true)
  }

  login = async (creds: UserFormValues) => {
    try {
      const user = await agent.Account.login(creds);
      store.commonStore.setToken(user.token); //set the token received in localStorage
      runInAction(() => (this.user = user)); //don't forget, if we're using async/await, we need to use this runInAction to set prop value state
      history.push('/activities'); //this directs user to the activities page
      store.modalStore.closeModal();
    } catch (error) {
      throw error;
    }
  };

  logout = () => {
    store.commonStore.setToken(null);
    window.localStorage.removeItem('jwt'); //remove the token from localStorage
    this.user = null;
    history.push('/'); //sends user back to main home page
  };

  getUser = async () => {
    try {
      const user = await agent.Account.current();
      runInAction(() => (this.user = user));
    } catch (error) {
      console.log(error);
    }
  };

  register = async (creds: UserFormValues) => {
    try {
      const user = await agent.Account.register(creds); //only difference from login ;)
      store.commonStore.setToken(user.token);
      runInAction(() => (this.user = user));
      history.push('/activities');
      store.modalStore.closeModal();
    } catch (error) {
      throw error;
    }
  };
}
