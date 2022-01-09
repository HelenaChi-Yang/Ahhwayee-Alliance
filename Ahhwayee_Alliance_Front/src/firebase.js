import firebase from 'firebase';
import 'firebase/storage';
require('firebase/firestore');

// Your web app's Firebase configuration
// var firebaseConfig = {
//   apiKey: 'AIzaSyDcNL_NKslNrxggDFeS98cEngNz4BKuOsI',
//   authDomain: 'vue-shop-19fa3.firebaseapp.com',
//   databaseURL: 'https://vue-shop-19fa3.firebaseio.com',
//   projectId: 'vue-shop-19fa3',
//   storageBucket: 'vue-shop-19fa3.appspot.com',
//   messagingSenderId: '234432297202',
//   appId: '1:234432297202:web:07f72bdf86d04f8ddb786b',
//   measurementId: 'G-43S50WF5KX',
// };

const firebaseConfig = {
  apiKey: 'AIzaSyDTFFg4Mf_ozu0FGIqy8Y6js7N1C9G2Yhg',
  authDomain: 'aweijiabawei.firebaseapp.com',
  projectId: 'aweijiabawei',
  storageBucket: 'aweijiabawei.appspot.com',
  messagingSenderId: '104225565119',
  appId: '1:104225565119:web:8db5886201fc84249c990a',
};
// Initialize Firebase

const fb = firebase.initializeApp(firebaseConfig);
const db = firebase.firestore();

const storage = firebase.storage();

export { fb, db, storage };
