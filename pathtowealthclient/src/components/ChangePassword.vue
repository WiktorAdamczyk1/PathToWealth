<template>
  <div class="alternate-section">
    <div class="setting-container">
      <h2>Change Password</h2>
      <el-form ref="formRef" :model="form" @submit.native.prevent="onChangePassword" label-position="top">
        <label class="form-item-label">Current Password</label>
        <el-form-item prop="currentPassword"
          :rules="[{ required: true, message: 'Current password is required' }]">
          <el-input v-model="form.currentPassword" type="password" placeholder="Current Password"></el-input>
        </el-form-item>
        <label class="form-item-label">New Password</label>
        <el-form-item prop="newPassword"
          :rules="[{ required: true, message: 'New password is required' }]">
          <el-input v-model="form.newPassword" type="password" placeholder="New Password"></el-input>
        </el-form-item>
        <el-button type="primary" native-type="submit">Change Password</el-button>
      </el-form>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, reactive, ref } from 'vue'
import { ElForm, ElFormItem, ElInput, ElButton } from 'element-plus'
import { changePassword } from '../requests';

export default defineComponent({
  components: {
    ElForm,
    ElFormItem,
    ElInput,
    ElButton
  },
  setup() {
    const form = reactive({
      currentPassword: '',
      newPassword: ''
    })
    const formRef = ref();

    const onChangePassword = () => {
      formRef.value.validate((valid: boolean) => {
        if (valid) {
          changePassword(form.currentPassword, form.newPassword)
            .then(() => {
              alert('Password changed successfully');
              form.currentPassword = '';
              form.newPassword = '';
            })
            .catch(console.error);
        } else {
          console.log('Validation failed');
        }
      });
    };

    return { form, onChangePassword, formRef }
  }
})
</script>

<style lang="scss">
.setting-container {
  display: flex;
  flex-direction: column;
  justify-content: center;
  padding: 2rem 3rem;
  margin: auto;
  width: 400px;
}
</style>