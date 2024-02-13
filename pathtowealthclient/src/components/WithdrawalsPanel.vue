<template>
    <div>
        <h2>Retirement prediction</h2>
        <p v-if="resultsStore.state.withdrawalAmounts.length">
            First amount withdrawn:
            {{ resultsStore.state.withdrawalAmounts[0].toLocaleString('en-US', { style: 'currency', currency: 'USD' }) }}
        </p>
        <p v-else>
            No withdrawal values available.
        </p>
    </div>
</template>
  
<script lang="ts" setup>
import resultsStore from '../stores/results'
</script>