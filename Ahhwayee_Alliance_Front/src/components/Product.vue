<template>
  <div class="product ">
    <div class="container-fluid">
      <!-- banner -->
      <!-- <div class=" d-flex justify-content-center mt-5">
        <div class="w-50">
          <div
            id="carouselExampleControls"
            class="carousel slide 0 "
            data-ride="carousel"
          >
            <div class="carousel-inner ">
              <div class="carousel-item active ">
                <img
                  src="../assets/images/carousel-1.jpg"
                  class="d-block w-100 img-fluid"
                  alt="..."
                />
              </div>
              <div class="carousel-item">
                <img
                  src="../assets/images/carousel-2.jpg"
                  class="d-block w-100"
                  alt="..."
                />
              </div>
              <div class="carousel-item">
                <img
                  src="../assets/images/carousel-3.jpg"
                  class="d-block w-100"
                  alt="..."
                />
              </div>
            </div>
            <button
              class="carousel-control-prev"
              type="button"
              data-target="#carouselExampleControls"
              data-slide="prev"
            >
              <span
                class="carousel-control-prev-icon"
                aria-hidden="true"
              ></span>
              <span class="sr-only">Previous</span>
            </button>
            <button
              class="carousel-control-next"
              type="button"
              data-target="#carouselExampleControls"
              data-slide="next"
            >
              <span
                class="carousel-control-next-icon"
                aria-hidden="true"
              ></span>
              <span class="sr-only">Next</span>
            </button>
          </div>
        </div>
      </div> -->
      <!-- main part -->
      <div class="row mt-5">
        <!-- 測攔 -->
        <div class="col-12 mb-3  col-md-2 ">
          <!-- 晃動愛心 -->
          <div class="h3 mb-5">
            <span class="shake-slow shake-constant">打賞幫補血</span>
            <span class="shake-slow shake-constant" style="white-space: nowrap;"
              >_(┐ ◟;ﾟдﾟ)ノ
              <b-icon
                icon="heart-fill"
                animation="throb"
                font-scale="1"
                color="#ff00ff"
              ></b-icon
            ></span>
          </div>

          <!-- 篩選按鈕 -->
          <div class="list-group mx-auto ">
            <!-- 又餓又病按鈕 -->
            <button
              type="button"
              class="list-group-item list-group-item-action px-0"
              aria-current="true"
              @click="sortByAll"
              :class="{ active: activeBtn === 'all' }"
            >
              <div>需要罐罐和醫療 <i class="fas fa-dollar-sign"></i></div>
            </button>
            <!-- 有點餓按鈕 -->
            <button
              type="button"
              class="list-group-item list-group-item-action px-0"
              aria-current="true"
              @click="sortByHungry()"
              :class="{ active: activeBtn === 'hungry' }"
            >
              <div>需要罐罐 <i class="fas fa-bone "></i></div>
            </button>
            <!-- 有點病按鈕 -->
            <button
              type="button"
              class="list-group-item list-group-item-action px-0"
              @click="sortBySick()"
              :class="{ active: activeBtn === 'sick' }"
            >
              <div>
                需要醫療 <i class="fas fa-briefcase-medical medical-icon"></i>
              </div>
            </button>
          </div>
          <!-- 跑馬燈 -->
          <div class="mt-5 w-100 d-none d-lg-block">
            <vue-seamless-scroll
              :data="List"
              class="seamless-warp "
              :class-option="classOption"
              ref="seamlessScroll"
            >
              <ul class="w-100">
                <li
                  class="DataList_top row mx-0 w-100 bg-info"
                  v-for="(item, index) in List"
                  :key="index"
                >
                  <div class="col-3 col-xl-3 p-0 d-none d-xl-block">
                    {{ item.account }}
                  </div>
                  <div class="col-6 col-xl-3 p-0">
                    <div>
                      <b-avatar
                        variant="info"
                        :badge-variant="
                          item.shelterId % 2 === 0 ? 'warning' : 'danger'
                        "
                        :src="
                          'https://placekitten.com/300/300?image=' +
                            item.shelterId
                        "
                        size="4rem"
                      >
                        <template #badge
                          ><b-icon
                            :icon="
                              item.shelterId % 2 === 0
                                ? 'star-fill'
                                : 'suit-heart-fill'
                            "
                          ></b-icon></template
                      ></b-avatar>
                    </div>
                  </div>
                  <div class="col-6 col-xl-6 p-0">
                    {{ item.shelterId % 2 === 0 ? '餵飽' : '醫治'
                    }}<strong>{{
                      Math.floor(item.shelterId + Math.sqrt(item.shelterId))
                    }}</strong
                    >隻 <br />毛寶
                  </div>
                </li>
              </ul>
            </vue-seamless-scroll>
          </div>

          <!-- 影片 -->
          <!-- <div>
            <iframe
              width="560"
              height="315"
              src="https://www.youtube.com/embed/8CrH_VGuY9c"
              title="YouTube video player"
              frameborder="0"
              allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
              allowfullscreen
            ></iframe>
          </div> -->
        </div>

        <!-- 卡片區 -->
        <div class="col-12 col-md-10 row mx-auto">
          <!-- 卡片列表 -->
          <div
            class=" col-md-4 col-lg-3  mb-3"
            v-for="(shelterCity, index) in shelterSortData"
            :key="shelterCity.shelterId"
          >
            <!-- 每張卡片 -->
            <div
              class="card product-item h-100"
              :style="{
                background:
                  index === 0
                    ? firstCardColor
                    : index === 1
                    ? secondCardColor
                    : index === 2
                    ? thirdCardColor
                    : 'white',
              }"
            >
              <!-- 收容所卡片狀態 -->
              <div class="card-body d-flex flex-column">
                <div class="card-title h5 my-3">
                  <div>
                    <strong>{{ shelterCity.shelterImgName }}</strong>
                    <br />
                    {{
                      transferPointsToOtherThings(shelterCity, cardHeadTitle)
                    }}
                  </div>
                </div>
              </div>

              <!-- 評級區 -->
              <b-form-rating
                id="rating-readonly"
                icon-empty="heart"
                icon-half="heart-half"
                icon-full="heart-fill"
                icon-clear="slash-circle"
                show-clear
                :value="howManyStars(shelterCity)"
                readonly
                class="mb-2"
                color="#ff00ff"
              ></b-form-rating>

              <div></div>

              <!-- 卡片幻燈片 -->
              <!-- <carousel
                id="pic"
                :perPage="1"
                :paginationPadding="3"
                :autoplay="true"
              >
                <slide v-for="(image, index) in shelterImages" :key="index">
                  <div class="shelterCity-img-list">
                    <img
                      :src="shelterCity.shelterImgUrl"
                      class="card-img-top shelterCity-img  "
                      alt="..."
                    />
                  </div>
                </slide>
              </carousel> -->

              <div class="shelterCity-img-list">
                <img
                  :src="shelterCity.shelterImgUrl"
                  class="card-img-top shelterCity-img  "
                  alt="..."
                />
              </div>

              <!-- new -->
              <!-- <div>
                <b-carousel
                  id="carousel-1"
                  v-model="slide"
                  :interval="4000"
                  controls
                  indicators
                  background="#ababab"
                  img-width="300"
                  img-height="300"
                  style="text-shadow: 1px 1px 2px #333;"
                  @sliding-start="onSlideStart"
                  @sliding-end="onSlideEnd"
                >
                  Slides with custom text
                  <b-carousel-slide :img-src="shelterCity.shelterImgUrl">
                    <div class="h1">{{ shelterCity.shelterImgName }}</div>
                  </b-carousel-slide>

                  Slides with image only
                  <b-carousel-slide
                    :img-src="shelterCity.shelterImgUrl"
                  ></b-carousel-slide>

                  Slides with img slot
                  Note the classes .d-block and .img-fluid to prevent browser default image alignment
                  <b-carousel-slide>
                    <template #img>
                      <img
                        class="d-block img-fluid w-100 shelterCity-img"
                        width="300"
                        height="300"
                        :src="shelterCity.shelterImgUrl"
                        alt="image slot"
                      />
                    </template>
                  </b-carousel-slide>
                </b-carousel>
              </div> -->

              <p class="card-text h3">
                <strong>{{ shelterCity.shelterImgName }}</strong>
              </p>

              <!-- 收容所卡片按鈕區-->
              <div class="card-body d-flex row">
                <!-- 收容所卡片資訊按鈕 -->
                <!-- <div class="col-6 ">
                  <a
                    class="btn mb-3 btn-info mx-auto w-75"
                    @click="
                      info(shelterCity);
                      shelterDataNeedFoodAndMedical;
                    "
                    >資訊</a
                  >
                </div> -->
                <div class="col-12 ">
                  <button
                    @click="donateInfo(shelterCity)"
                    class="btn btn-primary mb-3 mx-auto w-75 "
                    style="background-color:cadetblue; border-color: transparent;"
                  >
                    <div class="h3">
                      打賞
                      <b-icon
                        icon="heart-fill"
                        animation="throb"
                        font-scale="1"
                        color="#ff00ff"
                      ></b-icon>
                    </div>
                  </button>
                </div>

                <!-- 收容所卡片打賞按鈕 -->
                <!-- <AddToCart
                  :shelter-data="shelterCity"
                  :shelter-images="shelterImages"
                  class="col-6"
                >
                </AddToCart> -->
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- 點擊打賞按鈕的出現的燈箱 -->
    <div
      class="modal fade"
      id="miniCart"
      tabindex="-1"
      role="dialog"
      aria-labelledby="exampleModalLabel"
      aria-hidden="true"
    >
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <!-- 燈相 header -->
          <div class="modal-header">
            <p class="modal-title text-left h2" id="exampleModalLabel">
              {{ shelterCity.shelterName }}
            </p>
            <button
              type="button"
              class="close"
              data-dismiss="modal"
              aria-label="Close"
            >
              <span aria-hidden="true">&times;</span>
            </button>
          </div>

          <!-- 燈箱 body 上半部 -->
          <div class="modal-body row d-none d-sm-flex">
            <!-- 卡片幻燈片區 -->
            <div class="col-8">
              <!-- 新卡片頭貼 -->
              <!-- 圖片 -->
              <div class="product-avatar-list">
                <!-- <b-avatar
                  badge-variant="info"
                  :src="shelterCity.shelterImgUrl"
                  size="100%"
                >
                </b-avatar> -->

                <div class="shelterCity-img-list circle">
                  <img
                    :src="shelterCity.shelterImgUrl"
                    class="card-img-top card-img rounded-circle"
                    alt="..."
                  />
                </div>

                <!-- 來自XXX -->
                <b-avatar size="7rem" class="product-avatar">
                  <p class=" d-flex  flex-column m-0">
                    <strong class="danger h4 text-warning">{{
                      shelterCity.shelterImgName
                    }}</strong>
                    <span class=" small d-none d-sm-inline">來自</span>
                    <span class=" small d-none d-sm-inline">{{
                      shelterCity.shelterName
                    }}</span>
                  </p>
                </b-avatar>
              </div>
            </div>
            <div class="col-4">
              <div class="form-group my-3">
                <p class="h3 " v-if="token">
                  您有 <strong>{{ userRemainPoints }}</strong> 點
                </p>
                <label class="form-label">
                  <div><i class="fas fa-bone "></i></div>
                  <div>{{ transferPointToFoods }}</div></label
                >
                <input
                  type="number"
                  class="form-control"
                  placeholder="點數"
                  v-model="donationFoodPoints"
                />
              </div>

              <div class="form-group my-3">
                <button
                  type="button"
                  class="btn btn-primary"
                  style="background-color: cadetblue; border-color:transparent; color:rgb(5, 28, 34);"
                  @click="donateFood()"
                >
                  捐贈食物
                </button>
              </div>

              <div class="form-group my-3">
                <label class="form-label">
                  <div><i class="fas fa-briefcase-medical  "></i></div>
                  <div>{{ transferPointToMedical }}</div>
                </label>
                <input
                  type="number"
                  class="form-control"
                  placeholder="點數"
                  v-model="donationMedicalPoints"
                />
              </div>

              <div class="form-group my-3">
                <button
                  type="button"
                  class="btn btn-primary"
                  style="background-color: cadetblue; border-color:transparent; color:rgb(5, 28, 34);"
                  @click="donateMedical()"
                >
                  捐贈醫療
                </button>
              </div>
            </div>
          </div>

          <!-- 燈箱 body 上半部  手機板-->
          <div class="modal-body row d-flex d-sm-none">
            <!-- 卡片幻燈片區 -->
            <div class="col-12">
              <!-- 新卡片頭貼 -->
              <div class="product-avatar-list">
                <!-- <b-avatar
                  badge-variant="info"
                  :src="shelterCity.shelterImgUrl"
                  size="100%"
                >
                </b-avatar> -->
                <div class="shelterCity-img-list circle">
                  <img
                    :src="shelterCity.shelterImgUrl"
                    class="card-img-top card-img rounded-circle img-fluid"
                    alt="..."
                  />
                </div>

                <b-avatar size="7rem" class="product-avatar">
                  <p class=" d-flex  flex-column m-0">
                    <strong class="danger h4 text-warning">{{
                      shelterCity.shelterImgName
                    }}</strong>
                    <span class=" small d-none d-sm-inline">來自</span>
                    <span class=" small d-none d-sm-inline">{{
                      shelterCity.shelterName
                    }}</span>
                  </p>
                </b-avatar>
              </div>
            </div>
            <div class="col-12">
              <div class="form-group my-3">
                <p class="h3 " v-if="token">
                  您有 <strong>{{ userRemainPoints }}</strong> 點
                </p>
                <!--食物進度條 -->
                <div class="h6 mt-3">
                  <i class="fas fa-bone "></i> 還有
                  <strong>{{
                    checkNegative(
                      (
                        (1 -
                          shelterCity.shelterGetFood /
                            shelterCity.shelterNeedFood) *
                        shelterCity.realNumber
                      ).toFixed(2)
                    )
                  }}</strong>
                  隻肚子餓的毛寶
                </div>

                <b-progress
                  :max="shelterCity.shelterNeedFood"
                  height="1rem"
                  class="mb-3"
                >
                  <b-progress-bar
                    :value="shelterCity.shelterGetFood"
                    variant="danger"
                  >
                  </b-progress-bar>
                </b-progress>
                <label class="form-label"> </label>
                <input
                  type="number"
                  class="form-control"
                  placeholder="點數"
                  v-model="donationFoodPoints"
                />
              </div>

              <div class="form-group my-3 d-flex justify-content-between">
                <div>{{ transferPointToFoods }}</div>
                <button
                  type="button"
                  class="btn btn-primary"
                  style="background-color: cadetblue; border-color:transparent; color:rgb(5, 28, 34);"
                  @click="donateFood()"
                >
                  捐贈食物
                </button>
              </div>

              <div class="form-group my-3">
                <!--醫療進度條 -->
                <div class="h6 mt-3">
                  <i class="fas fa-briefcase-medical   "></i>
                  還有
                  <strong>{{
                    checkNegative(
                      (
                        (1 -
                          shelterCity.shelterGetMedical /
                            shelterCity.shelterNeedMedical) *
                        shelterCity.realNumber
                      ).toFixed(2)
                    )
                  }}</strong>
                  隻生病的毛寶
                </div>

                <b-progress :max="shelterCity.shelterNeedMedical" height="1rem">
                  <b-progress-bar
                    :value="shelterCity.shelterGetMedical"
                    variant="danger"
                  >
                  </b-progress-bar>
                </b-progress>
                <label class="form-label"> </label>
                <input
                  type="number"
                  class="form-control"
                  placeholder="點數"
                  v-model="donationMedicalPoints"
                />
              </div>

              <div class="form-group my-3 d-flex justify-content-between">
                <div>{{ transferPointToMedical }}</div>
                <button
                  type="button"
                  class="btn btn-primary"
                  style="background-color: cadetblue; border-color:transparent; color:rgb(5, 28, 34);"
                  @click="donateMedical()"
                >
                  捐贈醫療
                </button>
              </div>
            </div>
          </div>

          <!-- 燈箱 body 下半部 -->
          <div class="modal-body ">
            <!-- 進度條 -->
            <div class="d-none d-sm-block">
              <!-- 收容所進度條 -->
              <!--食物進度條 -->
              <div class="h6 mt-3">
                <i class="fas fa-bone "></i> 還有
                <strong>{{
                  checkNegative(
                    (
                      (1 -
                        shelterCity.shelterGetFood /
                          shelterCity.shelterNeedFood) *
                      shelterCity.realNumber
                    ).toFixed(2)
                  )
                }}</strong>
                隻肚子餓的毛寶
              </div>

              <b-progress
                :max="shelterCity.shelterNeedFood"
                height="1rem"
                class="mb-3"
              >
                <b-progress-bar
                  :value="shelterCity.shelterGetFood"
                  variant="danger"
                >
                </b-progress-bar>
              </b-progress>
              <!--醫療進度條 -->
              <div class="h6 mt-3">
                <i class="fas fa-briefcase-medical   "></i>
                還有
                <strong>{{
                  checkNegative(
                    (
                      (1 -
                        shelterCity.shelterGetMedical /
                          shelterCity.shelterNeedMedical) *
                      shelterCity.realNumber
                    ).toFixed(2)
                  )
                }}</strong>
                隻生病的毛寶
              </div>

              <b-progress :max="shelterCity.shelterNeedMedical" height="1rem">
                <b-progress-bar
                  :value="shelterCity.shelterGetMedical"
                  variant="danger"
                >
                </b-progress-bar>
              </b-progress>
            </div>

            <!-- 資訊區 -->
            <div class="mt-3">
              地址:
              <a
                :href="
                  `https://www.google.com.tw/maps/place/${shelterCity.address}`
                "
                target="_blank"
                title="Google Map"
              >
                {{ shelterCity.address }}</a
              ><br />
              電話: {{ shelterCity.shelterPhoneNumber }}<br />
              <span style="color: #d45345;"
                >更新時間:
                {{ new Date(shelterCity.updateTime).toLocaleString() }}</span
              ><br />
            </div>
          </div>

          <!-- <div class="modal-footer"> -->
          <!-- <button
              type="button"
              class="btn btn-secondary"
              data-dismiss="modal"
            >
              Continue shopping
            </button> -->
          <!-- 若為登入狀態則進入結帳頁面 -->
          <!-- <a
              class="btn btn-primary"
              href="#"
              v-if="login != null"
              @click="checkout"
              >Check your Cart</a
            > -->
          <!-- 若為未登入狀態則跳出登入視窗 -->
          <!-- <a
              class="btn btn-primary"
              data-toggle="modal"
              data-target="#login"
              @click="checkout"
              v-if="login === null"
              >Check your Cart</a
            > -->
          <!-- </div> -->
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { db } from '../firebase';
import $ from 'jquery';

// import AddToCart from './AddToCart.vue';

// import dogImg from '../assets/data/dogImg.json';
// import animalShelters from '../assets/data/animal-shelter.json';
// import shelterCities from '../assets/data/shelterCity.json';
import marquee from '../assets/data/marquee.json';

import vueSeamlessScroll from 'vue-seamless-scroll';
// import LeaderboardData from '../assets/data/leaderboardData.json';

export default {
  name: 'product',
  components: {
    vueSeamlessScroll,
    // AddToCart,
  },
  data() {
    return {
      products: [],
      product: {},
      productsImages: [0, 1, 2],
      shelterCity: {},
      // shelterCities: shelterCities.slice(0, 10),
      // animalShelters: animalShelters,
      activeBtn: 'all',
      shelterImages: [],
      shelterData: [],
      shelterNeedFood: [],
      shelterNeedMedical: [],
      donationFoodPoints: 0,
      donationMedicalPoints: 0,
      List: marquee,
      firstCardColor: '#dc3545',
      secondCardColor: '#e04b59',
      thirdCardColor: '#e4606d',
      thisWeekDonation: [],
      thisWeekDonationRandomNumber: Math.floor(Math.random() * 25) + 1,
      cardHeadTitle: '急需罐罐與醫療',
      shelterSortData: [],
      userRemainPoints: 0,
      token: localStorage.getItem('token'),
      // 動畫
      slide: 0,
      sliding: null,
    };
  },
  methods: {
    donate(shelterCity) {
      this.shelterCity = shelterCity;
      // console.log(this.shelterCity);
      $('#miniCart').modal('show');
    },
    getImage(images) {
      return images[0];
    },
    info(shelterCity) {
      $('#itemInfo').modal('show');

      this.shelterCity = shelterCity;
    },
    donateInfo(shelterCity) {
      if (!localStorage.getItem('token')) {
        this.userRemainPoints = 0;
        this.token = null;
      }
      $('#miniCart').modal('show');
      this.shelterCity = shelterCity;
      // console.log(this.shelterCity);
    },
    sortByAll() {
      let requestOptions = {
        method: 'GET',
        redirect: 'follow',
      };
      fetch(
        'https://finalproject-336509.appspot.com/api/shelter/shelterrank/0',
        requestOptions
      )
        .then((response) => response.json())
        .then((result) => {
          this.shelterData = result;

          fetch(
            'https://finalproject-336509.appspot.com/api/shelter/shelterrank/1',
            requestOptions
          )
            .then((response) => response.json())
            .then((result) => {
              this.shelterNeedFood = result;
              fetch(
                'https://finalproject-336509.appspot.com/api/shelter/shelterrank/2',
                requestOptions
              )
                .then((response) => response.json())
                .then((result) => {
                  this.shelterNeedMedical = result;
                  this.shelterData.forEach((itemShelterData) => {
                    // 食物
                    this.shelterNeedFood.forEach((itemShelterNeedFood) => {
                      if (
                        itemShelterData.shelterName ===
                        itemShelterNeedFood.shelterName
                      ) {
                        itemShelterData.shelterNeedFood =
                          itemShelterNeedFood.shelterNeedPoints;
                        itemShelterData.shelterGetFood =
                          itemShelterNeedFood.shelterGetPoints;
                      }
                    });

                    // 醫療
                    this.shelterNeedMedical.forEach(
                      (itemShelterNeedMedical) => {
                        if (
                          itemShelterData.shelterName ===
                          itemShelterNeedMedical.shelterName
                        ) {
                          itemShelterData.shelterNeedMedical =
                            itemShelterNeedMedical.shelterNeedPoints;
                          itemShelterData.shelterGetMedical =
                            itemShelterNeedMedical.shelterGetPoints;
                        }
                      }
                    );
                  });
                })
                .catch((error) => console.log('error', error));
            })
            .catch((error) => console.log('error', error));
        })
        .catch((error) => console.log('error', error));

      this.activeBtn = 'all';
      this.cardHeadTitle = '急需罐罐與醫療';
      this.shelterSortData = this.shelterData;
      this.shelterSortData.forEach((itemShelterData) => {
        // 食物
        this.shelterNeedFood.forEach((itemShelterNeedFood) => {
          if (itemShelterData.shelterName === itemShelterNeedFood.shelterName) {
            itemShelterData.shelterNeedFood =
              itemShelterNeedFood.shelterNeedPoints;
            itemShelterData.shelterGetFood =
              itemShelterNeedFood.shelterGetPoints;
          }
        });

        // 醫療
        this.shelterNeedMedical.forEach((itemShelterNeedMedical) => {
          if (
            itemShelterData.shelterName === itemShelterNeedMedical.shelterName
          ) {
            itemShelterData.shelterNeedMedical =
              itemShelterNeedMedical.shelterNeedPoints;
            itemShelterData.shelterGetMedical =
              itemShelterNeedMedical.shelterGetPoints;
          }
        });
      });
    },
    sortByHungry() {
      let requestOptions = {
        method: 'GET',
        redirect: 'follow',
      };
      fetch(
        'https://finalproject-336509.appspot.com/api/shelter/shelterrank/0',
        requestOptions
      )
        .then((response) => response.json())
        .then((result) => {
          this.shelterData = result;

          fetch(
            'https://finalproject-336509.appspot.com/api/shelter/shelterrank/1',
            requestOptions
          )
            .then((response) => response.json())
            .then((result) => {
              this.shelterNeedFood = result;
              fetch(
                'https://finalproject-336509.appspot.com/api/shelter/shelterrank/2',
                requestOptions
              )
                .then((response) => response.json())
                .then((result) => {
                  this.shelterNeedMedical = result;
                  this.shelterData.forEach((itemShelterData) => {
                    // 食物
                    this.shelterNeedFood.forEach((itemShelterNeedFood) => {
                      if (
                        itemShelterData.shelterName ===
                        itemShelterNeedFood.shelterName
                      ) {
                        itemShelterData.shelterNeedFood =
                          itemShelterNeedFood.shelterNeedPoints;
                        itemShelterData.shelterGetFood =
                          itemShelterNeedFood.shelterGetPoints;
                      }
                    });

                    // 醫療
                    this.shelterNeedMedical.forEach(
                      (itemShelterNeedMedical) => {
                        if (
                          itemShelterData.shelterName ===
                          itemShelterNeedMedical.shelterName
                        ) {
                          itemShelterData.shelterNeedMedical =
                            itemShelterNeedMedical.shelterNeedPoints;
                          itemShelterData.shelterGetMedical =
                            itemShelterNeedMedical.shelterGetPoints;
                        }
                      }
                    );
                  });
                })
                .catch((error) => console.log('error', error));
            })
            .catch((error) => console.log('error', error));
        })
        .catch((error) => console.log('error', error));

      this.activeBtn = 'hungry';
      this.cardHeadTitle = '急需罐罐';
      this.shelterSortData = this.shelterNeedFood;
      this.shelterSortData.forEach((itemShelterData) => {
        // 食物
        this.shelterNeedFood.forEach((itemShelterNeedFood) => {
          if (itemShelterData.shelterName === itemShelterNeedFood.shelterName) {
            itemShelterData.shelterNeedFood =
              itemShelterNeedFood.shelterNeedPoints;
            itemShelterData.shelterGetFood =
              itemShelterNeedFood.shelterGetPoints;
          }
        });

        // 醫療
        this.shelterNeedMedical.forEach((itemShelterNeedMedical) => {
          if (
            itemShelterData.shelterName === itemShelterNeedMedical.shelterName
          ) {
            itemShelterData.shelterNeedMedical =
              itemShelterNeedMedical.shelterNeedPoints;
            itemShelterData.shelterGetMedical =
              itemShelterNeedMedical.shelterGetPoints;
          }
        });
      });
    },
    sortBySick() {
      let requestOptions = {
        method: 'GET',
        redirect: 'follow',
      };
      fetch(
        'https://finalproject-336509.appspot.com/api/shelter/shelterrank/0',
        requestOptions
      )
        .then((response) => response.json())
        .then((result) => {
          this.shelterData = result;

          fetch(
            'https://finalproject-336509.appspot.com/api/shelter/shelterrank/1',
            requestOptions
          )
            .then((response) => response.json())
            .then((result) => {
              this.shelterNeedFood = result;
              fetch(
                'https://finalproject-336509.appspot.com/api/shelter/shelterrank/2',
                requestOptions
              )
                .then((response) => response.json())
                .then((result) => {
                  this.shelterNeedMedical = result;
                  this.shelterData.forEach((itemShelterData) => {
                    // 食物
                    this.shelterNeedFood.forEach((itemShelterNeedFood) => {
                      if (
                        itemShelterData.shelterName ===
                        itemShelterNeedFood.shelterName
                      ) {
                        itemShelterData.shelterNeedFood =
                          itemShelterNeedFood.shelterNeedPoints;
                        itemShelterData.shelterGetFood =
                          itemShelterNeedFood.shelterGetPoints;
                      }
                    });

                    // 醫療
                    this.shelterNeedMedical.forEach(
                      (itemShelterNeedMedical) => {
                        if (
                          itemShelterData.shelterName ===
                          itemShelterNeedMedical.shelterName
                        ) {
                          itemShelterData.shelterNeedMedical =
                            itemShelterNeedMedical.shelterNeedPoints;
                          itemShelterData.shelterGetMedical =
                            itemShelterNeedMedical.shelterGetPoints;
                        }
                      }
                    );
                  });
                })
                .catch((error) => console.log('error', error));
            })
            .catch((error) => console.log('error', error));
        })
        .catch((error) => console.log('error', error));

      this.activeBtn = 'sick';
      this.cardHeadTitle = '急需醫療';
      this.shelterSortData = this.shelterNeedMedical;
      this.shelterSortData.forEach((itemShelterData) => {
        // 食物
        this.shelterNeedFood.forEach((itemShelterNeedFood) => {
          if (itemShelterData.shelterName === itemShelterNeedFood.shelterName) {
            itemShelterData.shelterNeedFood =
              itemShelterNeedFood.shelterNeedPoints;
            itemShelterData.shelterGetFood =
              itemShelterNeedFood.shelterGetPoints;
          }
        });

        // 醫療
        this.shelterNeedMedical.forEach((itemShelterNeedMedical) => {
          if (
            itemShelterData.shelterName === itemShelterNeedMedical.shelterName
          ) {
            itemShelterData.shelterNeedMedical =
              itemShelterNeedMedical.shelterNeedPoints;
            itemShelterData.shelterGetMedical =
              itemShelterNeedMedical.shelterGetPoints;
          }
        });
      });
    },
    checkNegative(number) {
      return number <= 0 ? 0 : number;
    },
    getDogNum(shelterCity) {
      return this.animalShelters.filter((data) => {
        if (data.shelter_name === shelterCity.ShelterName)
          return data.animal_kind === '狗';
      }).length;
    },
    getCatNum(shelterCity) {
      return this.animalShelters.filter((data) => {
        if (data.shelter_name === shelterCity.ShelterName)
          return data.animal_kind === '貓';
      }).length;
    },
    transferPointsToOtherThings(shelterCity, otherThings) {
      // let shelterOneData = shelterCity;
      if (otherThings === '急需罐罐') {
        // let food = Math.floor(
        //   (1 - shelterCity.shelterGetFood / shelterCity.shelterNeedFood) *
        //     shelterCity.realNumber
        // );

        let percentage = 0;

        if (shelterCity.shelterGetPoints / shelterCity.shelterNeedPoints >= 1) {
          percentage = 1;
        } else {
          percentage =
            shelterCity.shelterGetPoints / shelterCity.shelterNeedPoints;
        }

        let food = Math.ceil((1 - percentage) * shelterCity.realNumber);

        return '尚有 ' + food + ' 位飢餓的同伴';
      } else if (otherThings === '急需醫療') {
        // let mediacl = Math.floor(points / 200000 + Math.sqrt(points));
        // let mediacl = Math.floor(
        //   (1 - shelterCity.shelterGetMedical / shelterCity.shelterNeedMedical) *
        //     shelterCity.realNumber
        // );

        let percentage = 0;

        if (shelterCity.shelterGetPoints / shelterCity.shelterNeedPoints >= 1) {
          percentage = 1;
        } else {
          percentage =
            shelterCity.shelterGetPoints / shelterCity.shelterNeedPoints;
        }

        let mediacl = Math.ceil((1 - percentage) * shelterCity.realNumber);
        return '尚有 ' + mediacl + ' 位生病的同伴';
      } else {
        // let all = Math.floor(points / 300000 + Math.sqrt(points));
        let all = shelterCity.realNumber;
        // let all = Math.floor(
        //   (1 - shelterCity.shelterGetPoints / shelterCity.shelterNeedPoints) *
        //     shelterCity.realNumber
        // );
        return '共有 ' + all + ' 位同伴';
      }
    },
    getAnimalsPics() {
      let dogImgArr = [];
      for (let i = 0; i < 3; i++) {
        let randomNum = Math.floor(Math.random() * 100) + 1;
        dogImgArr[i] = this.productsImages[randomNum];
      }
      this.shelterImages = dogImgArr;
    },
    donateFood() {
      if (localStorage.getItem('token') === null) {
        $('#miniCart').modal('hide');
        $('#login').modal('show');
        return;
      }

      if (this.donationFoodPoints <= 0) {
        window.Swal.fire({
          icon: 'warning',
          title: '您的愛心遠大於0',
        });
        return;
      }

      if (this.userRemainPoints < this.donationFoodPoints) {
        window.Swal.fire({
          icon: 'warning',
          title: '您的愛心需要加值',
        });
        return;
      }

      let token = JSON.parse(localStorage.getItem('token'));
      let bearerToken = 'Bearer ' + token;

      let account = JSON.parse(localStorage.getItem('account'));

      var myHeaders = new Headers();
      myHeaders.append('Authorization', bearerToken);
      myHeaders.append('Content-Type', 'application/json');

      var raw = JSON.stringify({
        account: account,
        donationPoints: parseInt(this.donationFoodPoints),
        shelterId: parseInt(this.shelterCity.shelterId),
        purposeId: 1,
      });

      var requestOptions = {
        method: 'PUT',
        headers: myHeaders,
        body: raw,
        redirect: 'follow',
      };

      fetch(
        'https://finalproject-336509.appspot.com/api/userdonation/donatePoints',
        requestOptions
      )
        .then((response) => response.json())
        .then((result) => {
          if (result.msg === '捐點成功') {
            window.Swal.fire({
              icon: 'success',
              title: '捐點成功',
            });
            this.userRemainPoints -= this.donationFoodPoints;
            this.shelterCity.shelterGetFood += parseInt(
              this.donationFoodPoints
            );
            this.donationFoodPoints = 0;
          } else {
            window.Swal.fire({
              icon: 'error',
              title: '請重新登入',
            });
            localStorage.clear();
            this.$router.go(0);
          }
        })
        .catch((error) => {
          console.log('error', error);
          localStorage.clear();
          this.$router.go(0);
        });
    },
    donateMedical() {
      if (localStorage.getItem('token') === null) {
        $('#miniCart').modal('hide');
        $('#login').modal('show');
        return;
      }

      if (this.donationMedicalPoints <= 0) {
        window.Swal.fire({
          icon: 'warning',
          title: '您的愛心遠大於0',
        });
        return;
      }

      if (this.userRemainPoints < this.donationMedicalPoints) {
        window.Swal.fire({
          icon: 'warning',
          title: '您的愛心需要加值',
        });
        return;
      }

      let token = JSON.parse(localStorage.getItem('token'));
      let bearerToken = 'Bearer ' + token;

      let account = JSON.parse(localStorage.getItem('account'));

      var myHeaders = new Headers();
      myHeaders.append('Authorization', bearerToken);
      myHeaders.append('Content-Type', 'application/json');

      var raw = JSON.stringify({
        account: account,
        donationPoints: parseInt(this.donationMedicalPoints),
        shelterId: parseInt(this.shelterCity.shelterId),
        purposeId: 2,
      });

      var requestOptions = {
        method: 'PUT',
        headers: myHeaders,
        body: raw,
        redirect: 'follow',
      };

      fetch(
        'https://finalproject-336509.appspot.com/api/userdonation/donatePoints',
        requestOptions
      )
        .then((response) => response.json())
        .then((result) => {
          if (result.msg === '捐點成功') {
            window.Swal.fire({
              icon: 'success',
              title: '捐點成功',
            });
            this.userRemainPoints -= this.donationMedicalPoints;
            this.shelterCity.shelterGetMedical += parseInt(
              this.donationMedicalPoints
            );
            this.donationMedicalPoints = 0;
          } else {
            window.Swal.fire({
              icon: 'error',
              title: '請重新登入',
            });
            localStorage.clear();
            this.$router.go(0);
          }
        })
        .catch((error) => {
          console.log('error', error);
          localStorage.clear();
          this.$router.go(0);
        });
    },
    goToLogin() {
      if (localStorage.getItem('token') === null) {
        $('#login').modal('show');
      }
    },
    howManyStars(shelterCity) {
      let all = 0;
      if (this.cardHeadTitle === '急需罐罐') {
        all =
          ((shelterCity.shelterNeedFood - shelterCity.shelterGetFood) /
            shelterCity.shelterNeedFood) *
          5;
      } else if (this.cardHeadTitle === '急需醫療') {
        all =
          ((shelterCity.shelterNeedMedical - shelterCity.shelterGetMedical) /
            shelterCity.shelterNeedMedical) *
          5;
      } else {
        all =
          ((shelterCity.shelterNeedPoints - shelterCity.shelterGetPoints) /
            shelterCity.shelterNeedPoints) *
          5;
      }

      return 5 - all;
    },
    // 動畫
    onSlideStart() {
      this.sliding = true;
    },
    onSlideEnd() {
      this.sliding = false;
    },
  },
  firestore() {
    return {
      products: db.collection('products'),
    };
  },

  created() {
    let dogImgArr = [];
    for (let i = 0; i < 3; i++) {
      let randomNum = Math.floor(Math.random() * 100) + 1;
      dogImgArr[i] = this.productsImages[randomNum];
    }
    this.shelterImages = dogImgArr;
  },
  mounted() {
    let requestOptions = {
      method: 'GET',
      redirect: 'follow',
    };
    fetch(
      'https://finalproject-336509.appspot.com/api/shelter/shelterrank/0',
      requestOptions
    )
      .then((response) => response.json())
      .then((result) => {
        this.shelterData = result;
        this.shelterSortData = result;

        fetch(
          'https://finalproject-336509.appspot.com/api/shelter/shelterrank/1',
          requestOptions
        )
          .then((response) => response.json())
          .then((result) => {
            this.shelterNeedFood = result;
            fetch(
              'https://finalproject-336509.appspot.com/api/shelter/shelterrank/2',
              requestOptions
            )
              .then((response) => response.json())
              .then((result) => {
                this.shelterNeedMedical = result;
                this.shelterData.forEach((itemShelterData) => {
                  // 食物
                  this.shelterNeedFood.forEach((itemShelterNeedFood) => {
                    if (
                      itemShelterData.shelterName ===
                      itemShelterNeedFood.shelterName
                    ) {
                      itemShelterData.shelterNeedFood =
                        itemShelterNeedFood.shelterNeedPoints;
                      itemShelterData.shelterGetFood =
                        itemShelterNeedFood.shelterGetPoints;
                    }
                  });

                  // 醫療
                  this.shelterNeedMedical.forEach((itemShelterNeedMedical) => {
                    if (
                      itemShelterData.shelterName ===
                      itemShelterNeedMedical.shelterName
                    ) {
                      itemShelterData.shelterNeedMedical =
                        itemShelterNeedMedical.shelterNeedPoints;
                      itemShelterData.shelterGetMedical =
                        itemShelterNeedMedical.shelterGetPoints;
                    }
                  });
                });
              })
              .catch((error) => console.log('error', error));
          })
          .catch((error) => console.log('error', error));
      })
      .catch((error) => console.log('error', error));

    if (!this.thisWeekDonation.length) {
      fetch(
        'https://finalproject-336509.appspot.com/api/userdonation/marquee',
        requestOptions
      )
        .then((response) => response.json())
        .then((result) => (this.thisWeekDonation = result))
        .catch((error) => console.log('error', error));
    }

    // 拿使用者點數
    if (localStorage.getItem('token')) {
      let token = JSON.parse(localStorage.getItem('token'));
      let account = JSON.parse(localStorage.getItem('account'));

      let myHeaders = new Headers();
      let bearerToken = 'Bearer ' + token;

      myHeaders.append('Authorization', bearerToken);

      let requestOptions = {
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
          let { remainingPoints } = result.userData;
          this.userRemainPoints = remainingPoints;
        })
        .catch((error) => console.log('error', error));
    } else {
      this.userRemainPoints = 0;
    }
  },
  computed: {
    // shelterDataNeedFoodAndMedical() {
    //   this.shelterData.forEach((itemShelterData) => {
    //     // 食物
    //     this.shelterNeedFood.forEach((itemShelterNeedFood) => {
    //       if (itemShelterData.shelterName === itemShelterNeedFood.shelterName) {
    //         itemShelterData.shelterNeedFood =
    //           itemShelterNeedFood.shelterNeedPoints;
    //         itemShelterData.shelterGetFood =
    //           itemShelterNeedFood.shelterGetPoints;
    //       }
    //     });

    //     // 醫療
    //     this.shelterNeedMedical.forEach((itemShelterNeedMedical) => {
    //       if (
    //         itemShelterData.shelterName === itemShelterNeedMedical.shelterName
    //       ) {
    //         itemShelterData.shelterNeedMedical =
    //           itemShelterNeedMedical.shelterNeedPoints;
    //         itemShelterData.shelterGetMedical =
    //           itemShelterNeedMedical.shelterGetPoints;
    //       }
    //     });
    //   });
    //   return this.shelterData;
    // },
    transferPointToFoods() {
      return `餵飽 ${(this.donationFoodPoints / 400).toFixed(2)} 隻毛寶`;
    },
    transferPointToMedical() {
      return `醫治 ${(this.donationMedicalPoints / 200).toFixed(2)} 隻毛寶`;
    },

    classOption() {
      return {
        step: 0.2, // 数值越大速度滚动越快
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

<style scoped lang="scss">
.list-group-item.active {
  background-color: rgb(227, 242, 253);
  color: black;
}
.product {
  background: #f2f2f2;
  padding-bottom: 3rem;
}

.modal-shelterCity-img {
  // width: 100%;
  // height: 100%;
  // max-height: 250px;
  width: 300px;
  height: 300px;
  object-fit: cover;
}

.shelterCity-img-list {
  // height: 300px;
}
.card-img {
  width: 300px;
  height: 300px;
  object-fit: cover;
}

.shelterCity-img {
  width: 200px;
  height: 200px;
  object-fit: cover;
}

.card-body {
  padding: 0;
}

.progress-text {
  color: black;
  position: absolute;
  z-index: 10;
  transform: translateX(1rem);
}

.food-icon {
  display: inline-block;
}

.medical-icon {
  display: inline-block;
}

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
  // width: 20%;
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

.product-avatar-list {
  position: relative;
}

.product-avatar {
  // transform: translate(5rem, -5rem);
  position: absolute;
  bottom: 0;
  left: 0;
}
</style>
