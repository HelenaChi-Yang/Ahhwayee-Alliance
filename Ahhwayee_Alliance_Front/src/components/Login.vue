<template>
  <div class="login">
    <!-- Modal -->
    <div
      class="modal fade"
      id="login"
      tabindex="-1"
      aria-labelledby="exampleModalLabel"
      aria-hidden="true"
    >
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal--navbar">
            <ul
              class="nav nav-fill nav-pills my-3 mx-2"
              id="pills-tab"
              role="tablist"
            >
              <li class="nav-item">
                <a
                  class="nav-link active"
                  id="pills-home-tab"
                  data-toggle="pill"
                  href="#pills-login"
                  role="tab"
                  aria-controls="pills-login"
                  aria-selected="true"
                  style="color: rgb(5, 28, 34);"
                  >登入</a
                >
              </li>
              <li class="nav-item">
                <a
                  class="nav-link"
                  id="pills-register-tab"
                  data-toggle="pill"
                  href="#pills-register"
                  role="tab"
                  aria-controls="pills-register"
                  aria-selected="false"
                  style="color:rgb(5, 28, 34);"
                  >註冊</a
                >
              </li>
            </ul>

            <div class="tab-content" id="pills-tabcontent">
              <!-- Login tag -->
              <div
                class="tab-pane fade show active"
                id="pills-login"
                role="tabpanel"
                aria-labelledby="pills-login-tab"
              >
                <h5 class="text-center">登入帳號</h5>
                <div class="login-input text-left mx-3 my-2">
                  <form @submit.prevent="login">
                    <div class="form-group">
                      <label for="login-account">帳號</label>
                      <input
                        type="text"
                        v-model="loginAccount"
                        class="form-control"
                        aria-describedby="emailHelp"
                        placeholder="輸入帳號"
                      />
                    </div>
                    <div class="form-group">
                      <label for="login-password">密碼</label>
                      <input
                        type="password"
                        @keyup.enter="login"
                        v-model="loginPassword"
                        class="form-control"
                        placeholder="密碼"
                      />
                    </div>
                    <button
                      class="btn btn-primary"
                      style="background-color: cadetblue; border-color:transparent ; color:rgb(5, 28, 34);"
                    >
                      登入
                    </button>
                  </form>
                </div>
              </div>

              <!-- Signup tag -->

              <div
                class="tab-pane fade"
                id="pills-register"
                role="tabpanel"
                aria-labelledby="pills-login-tab"
              >
                <h5 class="text-center">創建帳號</h5>

                <div class="login-input text-left mx-3 my-2">
                  <form @submit.prevent="checkAccountUnique">
                    <div class="form-group">
                      <label for="account">帳號</label>
                      <input
                        type="text"
                        v-model.trim="account"
                        class="form-control register-account"
                        placeholder="帳號名稱"
                        required
                      />
                    </div>

                    <button
                      class="btn btn-primary"
                      style="background-color: cadetblue; border-color:transparent; color:rgb(5, 28, 34);"
                    >
                      確認帳號可否使用
                    </button>
                  </form>

                  <form @submit.prevent="sendConfirmEmail">
                    <div class="form-group mt-3">
                      <label for="email">電子信箱</label>
                      <input
                        type="email"
                        v-model.trim="email"
                        class="form-control register-email"
                        aria-describedby="emailHelp"
                        placeholder="電子信箱"
                        required
                      />
                    </div>

                    <button
                      class="btn btn-primary"
                      style="background-color: cadetblue; border-color:transparent; color:rgb(5, 28, 34);"
                    >
                      取得驗證碼
                    </button>
                  </form>

                  <form @submit.prevent="register">
                    <div class="form-group mt-3">
                      <label for="verify-code">驗證碼</label>
                      <input
                        type="text"
                        v-model.trim="verifyCode"
                        class="form-control"
                        placeholder="驗證碼"
                        required
                      />
                    </div>

                    <div class="form-group">
                      <label for="password">密碼</label>
                      <input
                        type="password"
                        v-model.trim="password"
                        class="form-control"
                        placeholder="密碼"
                        required
                      />
                    </div>

                    <div class="form-group">
                      <label for="name">姓名</label>
                      <input
                        type="text"
                        v-model.trim="name"
                        class="form-control"
                        placeholder="姓名"
                        required
                      />
                    </div>

                    <button
                      class="btn btn-primary"
                      style="background-color: cadetblue; border-color:transparent; color:rgb(5, 28, 34);"
                    >
                      註冊
                    </button>
                  </form>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { fb, db } from '../firebase';
import $ from 'jquery';

export default {
  name: 'Login',
  data() {
    return {
      loginAccount: '',
      loginPassword: '',
      name: '',
      email: '',
      password: '',
      account: '',
      verifyCode: '',
    };
  },

  methods: {
    registerOld() {
      fb.auth()
        .createUserWithEmailAndPassword(this.email, this.password)
        .then((user) => {
          $('#login').modal('hide');
          this.$store.commit('getcurrentuser', user.user.uid);
          this.gotoadmin(user.user.uid); //判斷登入者是一般使用者還是管理者，並傳送至相對應會員畫面
          db.collection('profiles')
            .doc(user.user.uid)
            .set({
              //在使用者註冊時同時將資料儲存至firebase的database
              name: this.name,
              id: user.user.uid,
              role: 'user', //一般註冊用戶在註冊時就會自動被設為使用者
            })
            .then(function() {
              console.log('Document successfully written!');
            })
            .catch(function(error) {
              console.error('Error writing document: ', error);
            });
          this.$router.replace('admin').catch((e) => {
            console.log(e);
          });
        })
        .catch(function(error) {
          // Handle Errors here.
          var errorCode = error.code;
          var errorMessage = error.message;
          if (errorCode == 'auth/weak-password') {
            alert('The password is too weak.');
          } else {
            alert(errorMessage);
          }
          console.log(error);
        });
    },
    register() {
      var myHeaders = new Headers();
      myHeaders.append('Content-Type', 'application/json');

      var raw = JSON.stringify({
        account: this.account.trim(),
        password: this.password.trim(),
        userName: this.name.trim(),
        emailAddress: this.email.trim(),
        verifyCode: this.verifyCode.trim(),
      });

      var requestOptions = {
        method: 'POST',
        headers: myHeaders,
        body: raw,
        redirect: 'follow',
      };

      fetch(
        `https://finalproject-336509.appspot.com/api/auth/register`,
        requestOptions
      )
        .then((response) => response.json())
        .then((result) => {
          if (result.msg === '註冊成功') {
            window.Swal.fire({
              icon: 'success',
              title: '註冊成功',
              text: '歡迎光臨',
            });
            this.loginAccount = this.account;
            this.loginPassword = this.password;
            this.name = '';
            this.email = '';
            this.password = '';
            this.account = '';
            this.verifyCode = '';
          } else if (result.msg === '驗證碼錯誤') {
            window.Swal.fire({
              icon: 'error',
              title: '驗證碼錯誤',
            });
          } else {
            window.Swal.fire({
              icon: 'error',
              title: '註冊失敗',
            });
            console.log(result);
          }
        })
        .catch((error) => console.log('error', error));
    },
    checkAccountUnique() {
      var requestOptions = {
        method: 'GET',
        redirect: 'follow',
      };

      fetch(
        `https://finalproject-336509.appspot.com/api/auth/isaccountexist?account=${this.account.trim()}`,
        requestOptions
      )
        .then((response) => response.json())
        .then((result) => {
          if (result.msg === '帳號存在') {
            window.Swal.fire({
              icon: 'error',
              title: '已有此帳號',
            });
            return;
          } else if (result.msg === '帳號不存在') {
            $('.register-account').attr('disabled', true);
            window.Swal.fire({
              icon: 'success',
              title: '可以使用此帳號',
            });
          }
        })
        .catch((error) => console.log('error', error));
    },
    sendConfirmEmail() {
      var myHeaders = new Headers();
      myHeaders.append('Content-Type', 'application/json');

      var raw = JSON.stringify({
        account: this.account.trim(),
        emailaddress: this.email.trim(),
      });

      var requestOptions = {
        method: 'POST',
        headers: myHeaders,
        body: raw,
        redirect: 'follow',
      };

      fetch(
        'https://finalproject-336509.appspot.com/api/auth/sendemail',
        requestOptions
      )
        .then((response) => response.json())
        .then((result) => {
          if (result.msg === '驗證碼已寄出') {
            window.Swal.fire({
              icon: 'success',
              title: '驗證碼已寄出',
              text: '請至信箱領取驗證碼',
            });
            $('.register-account').attr('disabled', true);
            $('.register-email').attr('disabled', true);
          } else if (result.msg === '帳號已存在') {
            window.Swal.fire({
              icon: 'error',
              title: '帳號已存在',
            });
          } else {
            window.Swal.fire({
              icon: 'error',
              title: '此信箱已經領取過驗證碼',
            });
          }
        })
        .catch((error) => console.log('error', error));
    },
    login() {
      if (!this.loginAccount.trim() || !this.loginPassword.trim()) {
        window.Swal.fire({
          icon: 'error',
          title: '帳號密碼錯誤',
          text: '請輸入正確帳號密碼',
        });
        return;
      }
      var myHeaders = new Headers();
      myHeaders.append('Content-Type', 'application/json');

      var raw = JSON.stringify({
        account: this.loginAccount,
        password: this.loginPassword,
      });

      var requestOptions = {
        method: 'POST',
        headers: myHeaders,
        body: raw,
        redirect: 'follow',
      };

      fetch(
        'https://finalproject-336509.appspot.com/api/Auth/Login',
        requestOptions
      )
        .then((response) => response.json())
        .then((result) => {
          // Handle Errors here.

          //var errorMessage = error.message;
          if (result.msg === '登入成功') {
            localStorage.setItem('account', JSON.stringify(this.loginAccount));
            window.Swal.fire({
              icon: 'success',
              title: '登入成功',
              text: '歡迎光臨',
            });
            localStorage.setItem('token', JSON.stringify(result.token));
            $('#login').modal('hide');
            this.$router.replace('/adminforadmin/profile');
          } else {
            window.Swal.fire({
              icon: 'error',
              title: '帳號密碼錯誤',
              text: '請輸入正確帳號密碼',
            });
          }
        })
        .catch((error) => console.log('error', error));
    },
    loginOld() {
      fb.auth()
        .signInWithEmailAndPassword(this.email, this.password)
        .then((user) => {
          $('#login').modal('hide');
          this.$store.commit('getcurrentuser', user.user.uid); //儲存現登入使用者至localstorage
          this.gotoadmin(user.user.uid); //判斷登入者是一般使用者還是管理者，並傳送至相對應會員畫面
        })
        .catch(function(error) {
          // Handle Errors here.
          var errorCode = error.code;
          //var errorMessage = error.message;
          if (errorCode === 'auth/wrong-password') {
            window.Swal.fire({
              icon: 'error',
              title: 'Wrong password!',
            });
          } else {
            window.Swal.fire({
              icon: 'error',
              title: 'Oops...',
              text: "The user does't exist!",
            });
          }
          console.log(error);
        });
    },

    gotoadmin(user) {
      //從firebase的db得到當前登入者的身份並應相應地傳送到相應的使用者或管理者的會員中心
      let docRef = db.collection('profiles').doc(user);
      docRef
        .get()
        .then((doc) => {
          if (doc.exists) {
            var role = doc.data().role;
            return role;
          } else {
            console.log('No such document!');
          }
        })
        .then((role) => {
          if (role != 'user') {
            this.$router.replace('/adminforadmin');
          } else {
            this.$router.replace('/admin');
          }
        })
        .catch(function(error) {
          console.log('Error getting document:', error);
        });
    },
  },
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
.nav-pills .nav-link.active,
.nav-pills .show > .nav-link {
  color: #fff;
  background-color: cadetblue;
}
</style>
