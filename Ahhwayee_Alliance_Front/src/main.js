import Vue from 'vue'; //當我們install vue的同時也install webpack
import App from './App.vue';
import router from './router';
import { fb } from './firebase';
import store from './Store.js';
import 'popper.js';
import 'bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import { BootstrapVue, IconsPlugin } from 'bootstrap-vue';
import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap-vue/dist/bootstrap-vue.css';
import './assets/reset.scss';
import './assets/app.scss';
import '@fortawesome/fontawesome-free/css/all.css';
import '@fortawesome/fontawesome-free/js/all.js';
import 'jquery';
import Swal from 'sweetalert2';
import VueFirestore from 'vue-firestore';
import VueRouter from 'vue-router';
import Vue2Filters from 'vue2-filters';
import VueCarousel from 'vue-carousel';
import Vuex from 'vuex';

require('firebase/firestore');

Vue.use(VueCarousel);
Vue.use(Vuex);
Vue.use(VueFirestore);
Vue.use(VueRouter);
Vue.use(Vue2Filters);
// Install BootstrapVue
Vue.use(BootstrapVue);
// Optionally install the BootstrapVue icon components plugin
Vue.use(IconsPlugin);

Vue.config.productionTip = false;
Vue.config.devtools = true;

const Toast = Swal.mixin({
  toast: true,
  position: 'top-end',
  showConfirmButton: false,
  // timer: 3000,
  timer: 1000,
  timerProgressBar: true,
  onOpen: (toast) => {
    toast.addEventListener('mouseenter', Swal.stopTimer);
    toast.addEventListener('mouseleave', Swal.resumeTimer);
  },
});
window.Toast = Toast;
window.Swal = Swal;

let app = '';

fb.auth().onAuthStateChanged(function() {
  if (!app) {
    new Vue({
      router,
      store,
      render: (h) => h(App),
    }).$mount('#app');
  }
});

//金額格式
Vue.filter('currency', function(num) {
  return 'NTD ' + num;
});

const originalPush = VueRouter.prototype.push;
VueRouter.prototype.push = function push(location) {
  return originalPush.call(this, location).catch((err) => err);
};
