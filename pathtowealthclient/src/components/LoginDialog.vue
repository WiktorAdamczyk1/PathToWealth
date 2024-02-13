<template>
  <el-menu-item class="el-menu-item" @click="centerDialogVisible = true">
    <h style="text-align: center; width: 100%;">Sign in</h>
  </el-menu-item>

  <el-dialog
    v-model="centerDialogVisible"
    :title="isLogin ? 'Sign In' : 'Sign Up'"
    width="450"
    align-center
    style="border-radius: 1rem;"
  >
  <template #header>
    <img h="full" src="/LogoGreen.png" alt="Logo" style="height: 65px;"/>
    <h4 v-if="isLogin" style="margin: 0px 0px 0px 0px;">Sign In</h4>
    <h4 v-else style="margin: 0px 0px 0px 0px;">Sign Up</h4>
  </template>
    <div v-if="isLogin">
      <LoginWindow />
    </div>
    <div v-else>
      <RegisterWindow />
    </div>
    <el-divider />
    <span class="dialog-footer" style="align-items: center;" v-if="isLogin">
      <h>Don't have an account yet? </h><el-button class="dialog-change" type="text" @click="isLogin = false">Sign up</el-button>
    </span>
    <span class="dialog-footer" style="align-items: center;" v-else>
      <h>Already have an account? </h><el-button class="dialog-change" type="text" @click="isLogin = true">Sign in</el-button>
    </span>
  </el-dialog>
</template>
<script lang="ts" setup>
import { ref } from 'vue'

const centerDialogVisible = ref(false)
const isLogin = ref(true);
</script>

<style>
.invisible-button {
  opacity: 100;
  width: 100%;
  height: 100%;
  padding: 0;
  border: none;
}

.dialog-change {
  margin: 0;
  padding: 0;
  border: none;
  height: 0%;
}

.el-menu-item {
  color: var(--ep-text-color);
  text-align: center;
  border-radius: 10px;
  transition: background-color 0.3s ease;
  height: 50px;
}

.form-item-label {
    text-align: left;
    font-family: 'Poppins', sans-serif;
    letter-spacing: 0.005em;
    pointer-events: auto;
    box-sizing: border-box;
    display: inline-block;
    margin: 0;
    padding: 0;
    font-weight: 500;
    font-size: 12px;
    line-height: 18px;
    color: #525658;
    padding-bottom: 12px;
}

</style>