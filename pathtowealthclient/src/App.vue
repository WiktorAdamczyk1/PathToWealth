<template>
  <el-config-provider namespace="ep">
    <BaseHeader />
    <div class="main-container">
      <router-view />
    </div>
  </el-config-provider>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import store from './stores/user'

onMounted(async () => {
  await store.checkLogin();
});
</script>

<style>
#app {
  text-align: center;
  color: var(--ep-text-color-primary);
}

.main-container {
  height: calc(100vh - var(--ep-menu-item-height) - 3px);
}
</style>
