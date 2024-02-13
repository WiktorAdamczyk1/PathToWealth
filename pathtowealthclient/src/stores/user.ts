import { computed, reactive } from 'vue'
import * as Request from '../requests'

const state = reactive({
  username: '',
  email: '',

  error: ''
})

const getters = reactive({
  isLoggedIn: computed(() => state.username !== '')
})

const actions = {
  async login(usernameOrEmail: string, password: string) {
    const user = await Request.login(usernameOrEmail, password)
    if (user == null) {
      state.error = 'Could not find user.'
      return false
    }

    state.username = user.username
    state.email = user.email

    return true
  },
  async logout(event?: Event) {
    await Request.logout();
    state.username = '';
    state.email = '';
  },
  async checkLogin() {
    try {
      const user = await Request.getUser();
      if (user) {
        state.username = user.username;
        state.email = user.email;
        return true;
      }
    } catch (error) {
      console.error('Check login failed:', error);
    }
    state.username = '';
    state.email = '';
    return false;
  },
}

export default { state, getters, ...actions }