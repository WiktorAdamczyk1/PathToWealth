<template>
  <div>
    <apexchart :key="chartKey" type="line" :options="chartOptions" :series="chartSeries" height="350" />
  </div>
</template>

<script lang="ts">
import { watch, ref } from 'vue';
import VueApexCharts from 'vue3-apexcharts';
import resultsStore from '../stores/results';
import { isDark } from '../composables/dark';

export default {
  name: 'InvestmentChart',
  components: {
    apexchart: VueApexCharts,
  },
  setup() {
    const chartOptions = ref<{
      chart: {
        id: string;
        background: string;
      };
      theme: {
        mode: string;
      };
      xaxis: {
        categories: number[];
      };
      yaxis?: { 
        labels: { formatter: (val: number) => string } 
      };
      tooltip: {
        theme: string;
        formatter: (val: number) => string;
      };
    }>({
      chart: {
        id: 'investment-chart',
        background: 'transparent',
      },
      theme: {
        mode: isDark.value ? 'dark' : 'light',
      },
      xaxis: {
        categories: [],
      },
      tooltip: {
        theme: isDark.value ? 'dark' : 'light',
        formatter: (val: number) => {
          return val.toLocaleString('en-US', { style: 'currency', currency: 'USD' });
        },
      },
    });

    const chartSeries = ref<{
      name: string;
      data: number[];
      color: string;
    }[]>([
      { name: 'Future Value', data: [], color: '#0000FF' }, // Blue
      { name: 'Total Contributions', data: [], color: '#FFFF00' }, // Yellow
      { name: 'Total Interest', data: [], color: '#008000' }, // Green
      { name: 'Total Costs', data: [], color: '#FF0000' }, // Red
    ]);

    const updateTooltipTheme = () => {
      chartOptions.value.tooltip.theme = isDark.value ? 'dark' : 'light';
      chartOptions.value.theme.mode = isDark.value ? 'dark' : 'light';
      
    };

    watch(
      () => resultsStore.state,
      (newState) => {
        chartOptions.value.xaxis.categories = Array.from({ length: newState.totalMoneyAvailable.length }, (_, i) => i + 1 + resultsStore.state.startInvestmentYear - 1);
        chartSeries.value[0].data = newState.totalMoneyAvailable;
        chartSeries.value[1].data = newState.totalContributions;
        chartSeries.value[2].data = newState.totalInterestEarned;
        chartSeries.value[3].data = newState.totalCosts;

        // Format the tooltip values
        chartOptions.value.yaxis = {
          labels: {
            formatter: (val: number) => {
              return val.toLocaleString('en-US', { style: 'currency', currency: 'USD' });
            }
          }
        };
        chartKey.value++;
      },
      { deep: true }
    );

    // Update the tooltip theme when isDark changes and force the chart to re-render
    const chartKey = ref(0);
    watch(
      () => isDark.value,
      () => {
        updateTooltipTheme();
        chartKey.value++;
      }
    );

    return { chartOptions, chartSeries, chartKey };
  },
};
</script>