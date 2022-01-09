<template>
  <div class="about container-fluid">
    <div class="row no-gutters justify-content-center">
      <!-- 選擇所在區域 -->
      <div class="toolbox w-75  col-sm-6 p-2 bg-white">
        <!-- 選城市 -->
        <div class="form-group row ">
          <label for="cityName" class="col-sm-3 col-form-label text-left "
            >請選擇縣市:</label
          >
          <div class="col-sm-9">
            <select
              id="cityName"
              class="form-control"
              v-model="select.city"
              @change="
                cityChange();
                updateMap();
                showPopup(shelter[0].shelterId, 12);
              "
            >
              <option value="all">全部</option>
              <option :value="city" v-for="city in cities" :key="city">
                {{ city }}
              </option>
            </select>
          </div>
        </div>
        <!-- 選收容所 -->
        <div class="form-group row ">
          <label for="areaName" class="col-sm-3 col-form-label text-left"
            >請選擇動物收容所:</label
          >
          <div class="col-sm-9">
            <select
              id="areaName"
              class="form-control"
              v-model="select.store"
              @change="
                updateMap();
                showPopup(shelter[0].shelterId, 15);
              "
            >
              <option value="all">請選擇動物收容所</option>
              <option
                :value="a.shelterName"
                v-for="a in cityShelters"
                :key="a.shelterName"
              >
                {{ a.shelterName }}
              </option>
            </select>
          </div>
        </div>
        <!-- 各收容所資料 -->
        <div class="overflow-auto bd-links">
          <div
            class="card "
            :class="{
              highlight: a.realNumber / a.containMaxNumber < 0.5,
            }"
            v-for="a in shelter"
            :key="a.shelterName"
          >
            <div class="card-body" @click="showPopup(a.shelterId, 15)">
              <p class="card-title h3">
                <strong>{{ a.shelterName }}</strong>
              </p>

              <p class="card-text">
                <i class="fas fa-bone fa-3x"></i>
              </p>
              <b-progress
                :max="a.containMaxNumber"
                height="2rem"
                animated
                show-value
              >
                <b-progress-bar :value="a.realNumber">
                  <span class="progress-text"
                    >捐款點數進度:
                    <strong
                      >{{ a.realNumber }} / {{ a.containMaxNumber }}</strong
                    ></span
                  >
                </b-progress-bar>
              </b-progress>

              <p class="card-text">
                <i class="fas fa-briefcase-medical fa-3x"></i>
              </p>
              <b-progress
                :max="a.containMaxNumber"
                height="2rem"
                animated
                show-value
              >
                <b-progress-bar :value="a.realNumber">
                  <span class="progress-text"
                    >捐款點數進度:
                    <strong
                      >{{ a.realNumber }} / {{ a.containMaxNumber }}</strong
                    ></span
                  >
                </b-progress-bar>
              </b-progress>
            </div>
          </div>
        </div>
      </div>

      <!-- 顯示收容所位置 -->
      <div class="col-sm-6 w-75 ">
        <div id="map"></div>
      </div>
    </div>
    <Cart></Cart>
  </div>
</template>

<script>
import Cart from '../components/Cart';
import L from 'leaflet';
// import shelterData from '../assets/data/shelterCity.json';
import asData from '../assets/data/animal-shelter.json';

let openStreetMap = {};

export default {
  name: 'about',
  data: () => ({
    asAPI: 'Service/OpenData/TransService.aspx?UnitId=QcbUEzN6E6DL',
    asData,
    shelterData: [],
    animals: [],
    dataReady: false,
    select: {
      city: 'all',
      store: 'all',
    },
    markers: [],
  }),
  props: {
    msg: String,
  },
  components: {
    Cart,
  },
  computed: {
    cities() {
      return [...new Set(this.shelterData.map(({ city }) => city))];
    },
    cityShelters() {
      return this.shelterData.filter((shelter) => {
        if (this.select.city === 'all' && this.select.store === 'all') {
          return shelter.shelterName;
        } else {
          return shelter.city === this.select.city;
        }
      });
    },
    shelter() {
      return this.shelterData.filter((shelter) => {
        if (this.select.city === 'all' && this.select.store === 'all') {
          return shelter.shelterName;
        } else if (this.select.store === 'all') {
          return shelter.city === this.select.city;
        } else {
          return shelter.shelterName === this.select.store;
        }
      });
    },
  },
  watch: {
    shelter() {
      this.updateMap();
    },
  },
  methods: {
    cityChange() {
      this.select.store = 'all';
    },
    documentReady() {
      this.shelterData.forEach((data) => {
        const marker = L.marker([data.latitude, data.longitude], {
          title: data.shelterId,
        }).addTo(openStreetMap)
          .bindPopup(`<p><strong style="font-size: 20px;">${
          data.shelterName
        }</strong></p>
          地址: <a :href="https://www.google.com.tw/maps/place/${data.address}"
                target="_blank" title="Google Map">
                ${data.address}</a><br>
          電話: ${data.shelterPhoneNumber}<br>
          <span style="color: #d45345;">
          ${new Date(data.updateTime).toLocaleString()}</span><br>
          `);
        this.markers.push(marker);
      });
    },
    updateMap() {
      // clear markers
      openStreetMap.eachLayer((layer) => {
        if (layer instanceof L.Marker) {
          openStreetMap.removeLayer(layer);
          this.markers = [];
        }
      });

      const iconsConfig = {
        iconSize: [25, 41],
        iconAnchor: [12, 41],
        popupAnchor: [1, -34],
        shadowSize: [41, 41],
      };

      const icons = {
        green: new L.Icon({
          iconUrl:
            'https://cdn.rawgit.com/pointhi/leaflet-color-markers/master/img/marker-icon-2x-green.png',
          ...iconsConfig,
        }),
        red: new L.Icon({
          iconUrl:
            'https://raw.githubusercontent.com/pointhi/leaflet-color-markers/master/img/marker-icon-red.png',
          ...iconsConfig,
        }),
      };

      // add markers
      this.shelter.forEach((data) => {
        const icon =
          data.realNumber / data.containMaxNumber < 0.5
            ? icons.red
            : icons.green;
        // 透過藥局經緯度疊加標記
        const marker = L.marker([data.latitude, data.longitude], {
          title: data.shelterId,
          icon,
        }).addTo(openStreetMap)
          .bindPopup(`<p><strong style="font-size: 20px;">${
          data.shelterName
        }</strong></p>
          地址: <a href="https://www.google.com.tw/maps/place/${data.address}"
                target="_blank" title="Google Map">
                ${data.address}</a><br>
          電話: ${data.shelterPhoneNumber}<br>
          <span style="color: #d45345;">${new Date(
            data.updateTime
          ).toLocaleString()}</span><br>
          `);
        this.markers.push(marker);
      });
    },
    showPopup(id, zoom) {
      this.markers.forEach((marker) => {
        const markerID = marker.options.title;
        const position = marker.getLatLng();
        if (markerID === id) {
          openStreetMap.setView(position, zoom);
          marker.openPopup();
        }
      });
    },
  },
  mounted() {
    const requestOptions = {
      method: 'GET',
      redirect: 'follow',
    };

    fetch('https://finalproject-336509.appspot.com/api/shelter', requestOptions)
      .then((response) => response.json())
      .then((result) => (this.shelterData = result))
      .catch((error) => console.log('error', error));

    openStreetMap = L.map('map', {
      center: [24, 120.8],
      // 可以嘗試改變 zoom 的數值
      // 筆者嘗試後覺得 18 的縮放比例是較適當的查詢範圍
      zoom: 8,
    });

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution:
        'Map data &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors, <a href="https://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
      maxZoom: 20,
    }).addTo(openStreetMap);

    // this.documentReady();
  },
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
@import 'bootstrap/scss/bootstrap';

#map {
  position: relative;
  height: 100vh;
}

.bd-links {
  max-height: calc(100vh - 12.5rem);
  overflow-y: auto;
}

.card {
  cursor: pointer;
  background: #e9ffe3;
}

.card:hover {
  opacity: 0.7;
}

.progress-text {
  color: black;
  position: absolute;
  z-index: 10;
  transform: translateX(1rem);
}

.highlight {
  background: #ff4c4c;
}
</style>
