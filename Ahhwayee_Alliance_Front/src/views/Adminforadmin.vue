<template>
  <div class="adminforadmin ">
    <div class="page-wrapper default-theme sidebar-bg bg1 toggled ">
      <a
        id="show-sidebar"
        class="btn-sidebar btn-sm btn-dark pull-left"
        @click="closeMenu"
      >
        <i class="fas fa-bars"></i>
      </a>
      <nav id="sidebar" class="sidebar-wrapper ">
        <div class="sidebar-content ">
          <!-- sidebar-brand  -->
          <div class="sidebar-item sidebar-brand justify-content-end">
            <!-- <a href="#">pro sidebar</a> -->
            <a href="javascript:;" class="h3">{{ name }}</a>
            <div id="close-sidebar" @click="closeMenu">
              <i class="fas fa-times-circle"></i>
            </div>
          </div>
          <!-- sidebar-header  -->
          <div class="sidebar-item sidebar-header d-flex flex-nowrap">
            <div class="user-pic w-100">
              <img
                class="img-responsive img-rounded"
                :src="photoURL"
                alt="User picture"
              />
            </div>
            <div class="user-info">
              <!-- <span class="user-email">{{ email }}</span> -->
              <!-- <span
                class="admin"
                style="color:cadetblue; font-weight: bold;  text-align: left;"
                >ADMIN</span -->
              <!-- > -->
              <!-- <span class="user-status" style="text-align: left;">
                <i class="fa fa-circle"></i>
                <span style="text-align: left;">Online</span>
              </span> -->
            </div>
          </div>
          <!-- sidebar-menu  -->
          <div class=" sidebar-item sidebar-menu">
            <ul>
              <li class="header-menu">
                <span>選單</span>
              </li>
              <!-- <li>
                <router-link to="/adminforadmin/profile">
                  <i class="fa fa-book"></i>
                  <span class="menu-text">會員資訊</span>
                </router-link>
              </li> -->
              <!-- <li>
                <router-link to="/adminforadmin/addnewproduct">
                  <i class="fa fa-book"></i>
                  <span class="menu-text">Add new Products</span>
                </router-link>
              </li> -->
              <li>
                <router-link to="/">
                  <i class="fa fa-folder"></i>
                  <span class="menu-text">首頁</span>
                </router-link>
              </li>
              <li>
                <a href="javascript:;" @click="logout()">
                  <i class="fa fa-folder"></i>
                  <span class="menu-text">登出</span>
                </a>
              </li>
            </ul>
          </div>
        </div>
      </nav>
      <!-- sidebar開啟的網頁主要content  -->
      <main class="page-content">
        <router-view />
      </main>
    </div>
  </div>
</template>

<script>
// @ is an alias to /src
import $ from 'jquery';
import { db, fb } from '../firebase';

export default {
  name: 'adminforadmin',
  data() {
    return {
      profiles: [],
      name: null,
      email: null,
      photoURL: null,
    };
  },
  methods: {
    closeMenu() {
      $('.page-wrapper').toggleClass('toggled');
    },
    logoutOld() {
      this.$router.replace('/');
      fb.auth()
        .signOut()
        .then(() => {
          this.$router.replace('');
        })
        .catch((error) => {
          console.log(error);
        });
    },
    logout() {
      var myHeaders = new Headers();

      let token = JSON.parse(localStorage.getItem('token'));

      let account = JSON.parse(localStorage.getItem('account'));

      // function parseJwt(token) {
      //   var base64Url = token.split('.')[1];
      //   var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
      //   var jsonPayload = decodeURIComponent(
      //     atob(base64)
      //       .split('')
      //       .map(function(c) {
      //         return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
      //       })
      //       .join('')
      //   );
      //   return JSON.parse(jsonPayload);
      // }

      // let payload = parseJwt(token);

      myHeaders.append('Authorization', `Bearer ${token}`);
      myHeaders.append('Content-Type', 'application/json');

      var raw = JSON.stringify(account);

      var requestOptions = {
        method: 'POST',
        headers: myHeaders,
        body: raw,
        redirect: 'follow',
      };

      fetch(
        'https://finalproject-336509.appspot.com/api/Auth/ClearToken',
        requestOptions
      )
        .then((response) => response.json())
        .then((result) => {
          if (result.msg === 'Token已清除') {
            localStorage.removeItem('token');
            localStorage.removeItem('account');
            window.Toast.fire({
              icon: 'success',
              title: '登出成功',
            });
            this.login = null;
            this.$router.replace('/');
          } else {
            // window.Toast.fire({
            //   icon: 'error',
            //   title: '登出失敗',
            // });
            localStorage.clear();
            this.$router.replace('/');
            this.$router.go(0);
          }
        })
        .catch((error) => {
          console.log('error', error);
          localStorage.clear();
          this.$router.replace('/');
          this.$router.go(0);
        });
    },
  },

  firestore() {
    return {
      profiles: db.collection('profiles'),
    };
  },
  created() {
    // var user = fb.auth().currentUser;
    // this.email = user.email;
    // this.photoURL = user.photoURL;

    let token = JSON.parse(localStorage.getItem('token'));

    // function parseJwt(token) {
    //   var base64Url = token.split('.')[1];
    //   var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    //   var jsonPayload = decodeURIComponent(
    //     atob(base64)
    //       .split('')
    //       .map(function(c) {
    //         return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    //       })
    //       .join('')
    //   );

    //   return JSON.parse(jsonPayload);
    // }

    // let payload = parseJwt(token);

    let account = JSON.parse(localStorage.getItem('account'));

    var myHeaders = new Headers();
    let bearerToken = 'Bearer ' + token;

    myHeaders.append('Authorization', bearerToken);

    var requestOptions = {
      method: 'GET',
      headers: myHeaders,
      redirect: 'follow',
    };

    fetch(
      `https://finalproject-336509.appspot.com/api/userdonation/mydata?account=${account}`,
      requestOptions
    )
      .then((response) => {
        if (response.status !== 200) {
          localStorage.clear();
        }
        return response.json();
      })
      .then((result) => {
        let { emailAddress, userName } = result.userData;
        // let { emailAddress, userName,userImageUrl } = result.userData;
        this.email = emailAddress;
        // this.photoURL = userImageUrl;
        this.photoURL = 'https://i.pravatar.cc';
        this.name = userName;
      })
      .catch((error) => console.log('error', error));
  },
};
</script>
