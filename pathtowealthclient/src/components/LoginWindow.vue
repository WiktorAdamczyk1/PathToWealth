<template>
  <el-form @submit.prevent="onSubmit" :model="form" ref="formRef" label-width="120px" label-position="top">
    <el-form-item prop="username" :rules="[{ required: true, message: 'Username is required' }]">
      <label class="form-item-label">Username or Email:</label>
      <el-input v-model="form.username" placeholder="Username or Email" prefix-icon="user"></el-input>
    </el-form-item>
    <el-form-item prop="password" :rules="[{ required: true, message: 'Password is required' }]">
      <label class="form-item-label">Password:</label>
      <el-input v-model="form.password" type="password" placeholder="Password" prefix-icon="lock"></el-input>
    </el-form-item>
    <el-form-item>
      <el-button type="primary" @click="onSubmit" style="min-width: 100%; margin-left: 0;">Sign in</el-button>
    </el-form-item>
  </el-form>
</template>

<script lang="ts">
import { defineComponent, reactive, ref } from 'vue'
import userStore from '../stores/user'
import router from '../router';
import { ElMessage } from 'element-plus'

export default defineComponent({
  setup() {
    const form = reactive({
      username: '',
      password: ''
    });
    const formRef = ref();

    const onSubmit = () => {
    formRef.value.validate((valid: boolean) => {
        if (valid) {
            userStore.login(form.username, form.password).then((success) => {
                if (success) {
                    router.push('/');
                }
                form.username = '';
                form.password = '';
            }).catch(error => {
                ElMessage({
                    message: error.message,
                    type: 'error',
                });
            });
        } else {
            console.log('Validation failed');
        }
    });
};

    return { form, onSubmit, formRef }
  }
})

</script>