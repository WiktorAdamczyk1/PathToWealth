<template>
  <div class="setting-container">
    <h2>Delete Account</h2>
    <el-button type="danger" @click="onDeleteAccount">Delete Account</el-button>
  </div>  
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue'
import { deleteAccount } from '../requests';
import { useRouter } from 'vue-router';

export default defineComponent({
  setup() {
    const router = useRouter();
    const isLoggedIn = ref(true);

    const onDeleteAccount = () => {
      if (confirm('Are you sure you want to delete your account? This action cannot be undone.')) {
        deleteAccount()
          .then(() => {
            alert('Account deleted successfully');
            isLoggedIn.value = false; // Change the login state to false
            router.push('/'); // Redirect to the homepage
          })
          .catch(console.error);
      }
    };

    return { onDeleteAccount, isLoggedIn }
  }
})
</script>