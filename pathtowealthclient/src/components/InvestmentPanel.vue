<template>
    <div>
        <h2>Investment Results</h2>
        <p v-if="resultsStore.state.totalMoneyAvailable.length">
            Amount of money available at the end of the investing period:
            {{ resultsStore.state.totalMoneyAvailable[resultsStore.state.totalMoneyAvailable.length - 1].toLocaleString('en-US', { style: 'currency', currency: 'USD' }) }}
        </p>
        <p v-else>
            No investment values available.
        </p>
    </div>
</template>
  
<script lang="ts" setup>
import resultsStore from '../stores/results'
</script>