import Vue from 'vue';
import Router from 'vue-router';
import CreateUser from '@/components/CreateUser';
import ListUser from '@/components/ListUser';
import ListRequest from '@/components/ListRequest';

Vue.use(Router);

export default new Router({
  routes: [
    {
      path: '/',
      name: 'ListUser',
      component: ListUser,
    }, {
      path: '/create',
      name: 'CreateUser',
      component: CreateUser,
    }, {
      path: '/user/:namespace/:identifier',
      name: 'ListRequest',
      component: ListRequest,
    },
  ],
});
