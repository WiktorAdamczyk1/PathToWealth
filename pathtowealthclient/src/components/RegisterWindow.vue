<template>
  <el-form ref="registerFormRef" :model="registerForm" :rules="rules" class="demo-ruleForm" status-icon label-position="top">
    <el-form-item prop="username">
      <label class="form-item-label">Username</label>
      <el-input v-model="registerForm.username" placeholder="Username" prefix-icon="user" id="username" />
    </el-form-item>
    <el-form-item prop="email">
      <label class="form-item-label">Email</label>
      <el-input v-model="registerForm.email" placeholder="Email" prefix-icon="message" id="email" />
    </el-form-item>
    <el-form-item prop="password">
      <label class="form-item-label">Password</label>
      <el-input v-model="registerForm.password" type="password" placeholder="Password" prefix-icon="lock" id="password" />
    </el-form-item>
    <el-form-item prop="passwordConfirmation">
      <label class="form-item-label">Confirm Password</label>
      <el-input v-model="registerForm.passwordConfirmation" type="password" placeholder="Confirm Password" prefix-icon="lock" id="passwordConfirmation" />
    </el-form-item>
    <el-form-item>
      <el-button type="primary" @click="submitForm(registerFormRef)" style="min-width: 100%; margin-left: 0;">Sign up</el-button>
    </el-form-item>
  </el-form>
</template>
  
<script lang="ts" setup>
import { reactive, ref } from 'vue'
import type { FormInstance, FormRules } from 'element-plus'
import { register } from '../requests'
import type { RegisterForm } from '../requests'
import { ElMessage } from 'element-plus'

const formSize = ref('default')
const registerFormRef = ref<FormInstance>()
const registerForm = reactive<RegisterForm>({
  username: '',
  email: '',
  password: '',
  passwordConfirmation: '',
})

const rules = reactive<FormRules<RegisterForm>>({
  username: [
    { required: true, message: 'Please input username', trigger: 'blur' },
  ],
  email: [
    { required: true, message: 'Please input email', trigger: 'blur' },
    { type: 'email', message: 'Please input valid email', trigger: 'blur' },
  ],
  password: [
    { required: true, message: 'Please input password', trigger: 'blur' },
    { min: 5, message: 'Password should be at least 5 characters', trigger: 'blur' },
    { pattern: /[A-Z]/, message: 'Password should contain at least one uppercase letter', trigger: 'blur' },
    { pattern: /[a-z]/, message: 'Password should contain at least one lowercase letter', trigger: 'blur' },
    { pattern: /[0-9]/, message: 'Password should contain at least one number', trigger: 'blur' },
    { pattern: /[\W_]/, message: 'Password should contain at least one special character', trigger: 'blur' },
  ],
  passwordConfirmation: [
    { required: true, message: 'Please input password confirmation', trigger: 'blur' },
    {
      validator: (rule, value, callback) => {
        if (value !== registerForm.password) {
          callback(new Error('Password confirmation does not match password'))
        } else {
          callback()
        }
      }, trigger: 'blur'
    },
  ],
})

const submitForm = async (formEl: FormInstance | undefined) => {
  if (!formEl) return
  await formEl.validate((valid, fields) => {
    if (valid) {
      register(registerForm)
        .then(data => {
          console.log('Registration successful!', data)
          ElMessage({
            message: 'Registration successful!',
            type: 'success',
          })
        })
        .catch(error => {
          console.error('Registration failed!', error)
          ElMessage({
            message: 'Registration failed!',
            type: 'error',
          })
        })
    } else {
      console.log('error submit!', fields)
    }
  })
}


const resetForm = (formEl: FormInstance | undefined) => {
  if (!formEl) return
  formEl.resetFields()
}
</script>

<style>

el-form-item {

  label {
    
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
}

</style>
