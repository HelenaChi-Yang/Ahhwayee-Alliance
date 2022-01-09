<template>
  <div id="Leaderboard">
    <Navbar></Navbar>
    <div class="container-fluid">
      <div class="row justify-content-around" v-if="token">
        <LeaderboardChart class="col-12 " :leaderboard-chart-title-num="3" />
      </div>
      <div class="row justify-content-around">
        <LeaderboardChart
          class="col-12 col-md-4"
          :leaderboard-chart-title-num="0"
        />

        <LeaderboardChart
          class="col-12 col-md-4"
          :leaderboard-chart-title-num="1"
        />

        <LeaderboardChart
          class="col-12 col-md-4"
          :leaderboard-chart-title-num="2"
        />
      </div>

      <!-- <div class="row justify-content-center scollbox">
        <vue-seamless-scroll
          :data="List"
          class="seamless-warp col-10"
          :class-option="classOption"
          ref="seamlessScroll"
        >
          <ul>
            <li
              class="DataList_top"
              v-for="item in personalData"
              :key="item.donationId"
            >
              <div class="DataList_left one">{{ item.donationId }}</div>
              <div class="DataList_left two">{{ item.donationPoints }}</div>
              <div class="DataList_left three">
                <div class="user__avatar">
                  <img
                    :src="
                      'https://i.pravatar.cc/150?img=' +
                        (Math.floor(Math.random() * 25) + 1)
                    "
                  />
                </div>
              </div>
              <div class="DataList_left four">
                {{ new Date(item.donationTime).toLocaleString() }}
              </div>
              <div class="DataList_left five">{{ item.purposeId }}</div>
            </li>
          </ul>
        </vue-seamless-scroll>
      </div> -->
    </div>
    <Cart></Cart>
  </div>
</template>

<script>
import Navbar from '../components/Navbar';
import Cart from '../components/Cart';

// import vueSeamlessScroll from 'vue-seamless-scroll';
import LeaderboardData from '../assets/data/leaderboardData.json';
import LeaderboardChart from '../components/LeaderboardChart.vue';

export default {
  name: 'aboutview',
  components: {
    Navbar,
    // vueSeamlessScroll,
    LeaderboardChart,
    Cart,
  },
  data() {
    return {
      List: [],
      leaderboardChartData: {},
      personalData: [],
      token: JSON.parse(localStorage.getItem('token')),
    };
  },
  created() {
    this.List = LeaderboardData;
  },
  mounted() {
    let token = JSON.parse(localStorage.getItem('token'));
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
      .then((response) => response.json())
      .then((result) => {
        this.personalData = result.allDonation;
      })
      .catch((error) => console.log('error', error));
  },

  computed: {
    classOption() {
      return {
        step: 1, // 数值越大速度滚动越快
        limitMoveNum: 4, // 开始无缝滚动的数据量 this.dataList.length
        hoverStop: true, // 是否开启鼠标悬停stop
        direction: 1, // 0向下 1向上 2向左 3向右
        openWatch: true, // 开启数据实时监控刷新dom
        singleHeight: 0, // 单步运动停止的高度(默认值0是无缝不停止的滚动) direction => 0/1
        singleWidth: 0, // 单步运动停止的宽度(默认值0是无缝不停止的滚动) direction => 2/3
        waitTime: 1000, // 单步运动停止的时间(默认值1000ms)
        autoPlay: true,
      };
    },
  },
};
</script>

<style lang="scss" scoped>
.textStyle {
  font-size: 32px;
  font-weight: 700;
}

.seamless-warp {
  width: 100%;
  height: 50vh;
  overflow: hidden;
}
ul {
  padding: 0;
}
.DataList_top {
  list-style: none;
  width: 100%;
  height: 4rem;
  background-color: #4383c1;
  margin-top: 0.5rem;
  color: white;
  font-size: 1rem;
  display: flex;
  align-items: center;
}
.DataList_left {
  float: left;
  text-align: center;
}
.one {
  width: 20%;
}
.two {
  width: 20%;
  // margin-left: 1%;
}
.three {
  width: 20%;
  // margin-left: 1%;
}
.four {
  width: 20%;
  // margin-left: 1%;
}
.five {
  width: 20%;
  // margin-left: 1%;
  color: greenyellow;
}

.user__avatar {
  border-radius: 50%;
  height: 64px;
  overflow: hidden;
  position: relative;
  width: 64px;
  z-index: 9;
}

.user__avatar img {
  height: 100%;
  object-fit: cover;
  transition: transform 0.3s ease-in-out, filter 0.3s ease-in;
  width: 100%;
}

.user__avatar:hover img {
  filter: blur(1px) brightness(0.75);
  transform: scale(1.05);
}
</style>
