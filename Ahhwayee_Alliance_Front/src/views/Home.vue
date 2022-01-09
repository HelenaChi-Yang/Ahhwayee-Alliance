<template>
  <div class="home">
    <Navbar></Navbar>
    <!-- <Hero></Hero> -->
    <ProductList></ProductList>
    <!-- <About></About> -->
    <Login></Login>
  </div>
</template>

<script>
// @ is an alias to /src
// import Hero from '@/components/Hero.vue';
import ProductList from '@/components/ProductList.vue';
import Login from '@/components/Login.vue';
// import About from '@/components/About.vue';
import Navbar from '@/components/Navbar.vue';

export default {
  name: 'Home',
  data() {
    return {
      testData: [],
    };
  },
  components: {
    // Hero,
    ProductList,
    Login,
    // About,
    Navbar,
  },
  mounted() {
    fetch('https://warm-helix-333707.appspot.com/api/shelter/')
      .then((res) => res.json())
      .then((data) => (this.testData = data))
      .catch((err) => console.log(err.message));
  },
};
</script>
<style>
.graphic img {
  width: 100%;
  height: auto;
}
.col {
  padding-top: 5px;
}
</style>
