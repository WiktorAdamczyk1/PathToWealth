import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router';
import store from '../stores/user';

import Home from '../views/Home.vue';
import Account from '../views/Account.vue';

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/account',
    name: 'Account',
    component: Account,
    meta: {
      requiresAuth: true
    }
  }
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

router.beforeEach(async (to, from, next) => {
  const isLoggedIn = await store.checkLogin();

  if (to.meta.requiresAuth && !isLoggedIn) {
    next({ name: 'Login' });
  } else if (to.name === 'Login' && isLoggedIn) {
    next({ name: 'Home' });
  } else {
    next();
  }
});

export default router;
