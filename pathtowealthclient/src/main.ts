import { createApp } from "vue";
import App from "./App.vue";
import router from './router';
import ElementPlus from "element-plus";
import * as ElementPlusIconsVue from '@element-plus/icons-vue';

import "~/styles/index.scss";
import "uno.css";

import "element-plus/theme-chalk/src/message.scss";

const app = createApp(App);
app.use(ElementPlus);
app.use(router);
app.mount("#app");
for (const [key, component] of Object.entries(ElementPlusIconsVue)) {
    app.component(key, component)
  }
