<template>
  <div style="display: flex; flex-wrap: wrap;">
    <div class="text-container" style="flex: 1;">
      <h1 style="margin-top: 0px;">Investment Calculator</h1>
      <span class="text">Fill out the form with your information or use the default values based on the "The Simple Path
        to Wealth" by JL Collins. The two graphs will show the growth of your investment over time and the the amount of money you are expected to
        withdraw each year during retirement. The retirement strategy, is based on the "4% rule" which states that you can withdraw 4% of your initial investment
        each year during retirement and adjust for inflation to maintain the spending power. This strategy has a 96% success
        rate over 30 years of retirement.
      </span>
    </div>
    <div class="form-container">
      <el-form ref="userFinancialDataFormRef" :model="userFinancialDataForm" :rules="rules" label-width="120px"
        class="data-form" status-icon label-position="top">
        <div class="form-item">
          <div>
            <label class="form-item-label">Initial Deposit</label>
            <el-form-item prop="initialInvestment">
              <el-input v-model.number="userFinancialDataForm.initialInvestment" placeholder="$"
                :formatter="currencyFormatter" :parser="currencyParser" />
            </el-form-item>
          </div>
          <div>
            <label class="form-item-label">Contribution amount</label>
            <el-form-item prop="yearlyOrMonthlySavings">
              <el-input v-model.number="userFinancialDataForm.yearlyOrMonthlySavings" placeholder="$"
                :formatter="currencyFormatter" :parser="currencyParser" />
            </el-form-item>
          </div>
        </div>
        <div class="form-item">
          <div style="width: 100%;">
            <label class="form-item-label">Contribution frequency</label>
            <el-form-item prop="isInvestmentMonthly">
              <div class="form-item-buttons">
                <el-button
                  style="margin-left: 0px; margin-right: 0px; border-top-right-radius: 0px; border-bottom-right-radius: 0px; border-top-left-radius: 2rem; border-bottom-left-radius: 2rem;"
                  type="primary" class="frequency-button" :plain="userFinancialDataForm.isInvestmentMonthly"
                  @click="userFinancialDataForm.isInvestmentMonthly = false">
                  Annually
                </el-button>
                <el-button
                  style="margin-left: 0px; margin-right: 0px; border-top-right-radius: 2rem; border-bottom-right-radius: 2rem; border-top-left-radius: 0px; border-bottom-left-radius: 0px;"
                  type="primary" class="frequency-button" :plain="!userFinancialDataForm.isInvestmentMonthly"
                  @click="userFinancialDataForm.isInvestmentMonthly = true">
                  Monthly
                </el-button>
              </div>
            </el-form-item>
          </div>
        </div>
        <div class="form-item">
          <div>
            <label class="form-item-label">Beginning of Investing</label>
            <el-form-item prop="startInvestmentYear">
              <el-input v-model.number="userFinancialDataForm.startInvestmentYear" placeholder="Year" />
            </el-form-item>
          </div>
          <div>
            <label class="form-item-label">Year of Retirement</label>
            <el-form-item prop="startWithdrawalYear">
              <el-input v-model.number="userFinancialDataForm.startWithdrawalYear" placeholder="Year" />
            </el-form-item>
          </div>
        </div>
        <div class="form-item">
          <div>
            <label class="form-item-label">Annual return rate of Stock fund</label>
            <el-form-item prop="stockAnnualReturn">
              <el-input-number v-model="userFinancialDataForm.stockAnnualReturn" :min="0" :max="200" label="Percentage"
                suffix="%" placeholder="%" />
            </el-form-item>
          </div>
          <div>
            <label class="form-item-label">Annual return rate of Bond fund</label>
            <el-form-item prop="bondAnnualReturn">
              <el-input-number v-model="userFinancialDataForm.bondAnnualReturn" :min="0" :max="200" label="Percentage"
                suffix="%" placeholder="%" />
            </el-form-item>
          </div>
        </div>
        <div class="form-item">
          <div style="width: 100%;">
            <el-button class="advanced-button" @click="toggleAdvancedOptions">
              {{ showAdvancedOptions ? 'Hide Advanced Options' : 'Show Advanced Options' }}
            </el-button>
          </div>
        </div>
        <Vue3SlideUpDown v-model="showAdvancedOptions" style="width: 100%;">
          <!-- Advanced form -->
          <div class="form-item">
            <div>
              <label class="form-item-label">Stock fund cost Ratio</label>
              <el-form-item prop="stockCostRatio">
                <el-input-number v-model="userFinancialDataForm.stockCostRatio" :min="0" :max="100" label="Percentage"
                  suffix="%" placeholder="%" />
              </el-form-item>
            </div>
            <div>
              <label class="form-item-label">Bond fund cost Ratio</label>
              <el-form-item prop="bondCostRatio">
                <el-input-number v-model="userFinancialDataForm.bondCostRatio" :min="0" :max="100" label="Percentage"
                  suffix="%" placeholder="%" />
              </el-form-item>
            </div>
          </div>
          <div class="form-item">
            <div>
              <label class="form-item-label">Stock to Bond Ratio</label>
              <el-form-item prop="stockToBondRatio">
                <el-input-number v-model="userFinancialDataForm.stockToBondRatio" :min="0" :max="100" label="Percentage"
                  suffix="%" placeholder="%" />
              </el-form-item>
            </div>
            <div>
              <label class="form-item-label">Years of retirement</label>
              <el-form-item prop="retirementDuration">
                <el-input-number v-model="userFinancialDataForm.retirementDuration" :min="0" :max="100" label="Percentage"
                  suffix="%" placeholder="%" />
              </el-form-item>
            </div>
          </div>
          <div class="form-item">
            <div>
              <label class="form-item-label">Expected Inflation rate</label>
              <el-form-item prop="inflationRate">
                <el-input-number v-model="userFinancialDataForm.inflationRate" :min="0" :max="100" label="Percentage"
                  suffix="%" placeholder="%" />
              </el-form-item>
            </div>
            <div>
              <label class="form-item-label">Withdrawal rate during retirement</label>
              <el-form-item prop="withdrawalRate">
                <el-input-number v-model="userFinancialDataForm.withdrawalRate" :min="0" :max="100" label="Percentage"
                  suffix="%" placeholder="%" />
              </el-form-item>
            </div>
          </div>
        </Vue3SlideUpDown>
        <div class="form-item">
          <div style="padding-top: 15px; width: 100%;">
            <el-button type="primary" @click="submitForm(userFinancialDataFormRef)" style="width: 100%;">Calculate</el-button>
          </div>
        </div>
      </el-form>
    </div>
  </div>
</template>

<script lang="ts">
import { Vue3SlideUpDown } from "vue3-slide-up-down";

export default {
  data() {
    return {
      showAdvancedOptions: false,
    }
  },
  methods: {
    toggleAdvancedOptions() {
      this.showAdvancedOptions = !this.showAdvancedOptions;
    },
    currencyFormatter(value: number): string {
      return new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD', minimumFractionDigits: 0, maximumFractionDigits: 0 }).format(value);
    },
    currencyParser(value: string): number {
      return parseFloat(value.replace(/\$\s?|(,*)/g, ''));
    },
    percentageFormatter(value: string): string {
      return value + '%';
    },
    percentageParser(value: string): string {
      return value.replace('%', '');
    },
  },
}

</script>

<script lang="ts" setup>
import { reactive, ref, onMounted } from 'vue'
import type { FormInstance, FormRules } from 'element-plus'
import { getUserFinancialData, updateUserFinancialData } from '../requests'
import type { UserFinancialData } from '../requests'
import userStore from '../stores/user'
import resultsStore from '../stores/results'

const userFinancialDataFormRef = ref()
const currentYear = new Date().getFullYear();
const userFinancialDataForm = reactive<UserFinancialData>({
  initialInvestment: 10000,
  startInvestmentYear: currentYear,
  startWithdrawalYear: currentYear + 30,
  isInvestmentMonthly: false,
  yearlyOrMonthlySavings: 12000,
  stockAnnualReturn: 7.90,
  stockCostRatio: 0.04,
  bondAnnualReturn: 3.30,
  bondCostRatio: 0.05,
  stockToBondRatio: 100,
  retirementDuration: 30,
  inflationRate: 2.50,
  withdrawalRate: 4.00
})

const rules = reactive<FormRules<UserFinancialData>>({
  initialInvestment: [
    { type: 'number', required: true, message: 'Initial investment must be a number and is required', trigger: 'blur' },
    { type: 'number', min: 0, message: 'Initial investment must be greater than or equal to 0', trigger: 'blur' }
  ],
  startInvestmentYear: [
    { type: 'number', required: true, message: 'Start investment year must be a number and is required', trigger: 'blur' },
    { type: 'number', min: 2000, max: 2200, message: 'Start investment year must be between 2000 and 2200', trigger: 'blur' }
  ],
  startWithdrawalYear: [
    { type: 'number', required: true, message: 'Start withdrawal year must be a number and is required', trigger: 'blur' },
    { type: 'number', min: 2001, max: 2230, message: 'Start withdrawal year must be between 2001 and 2230', trigger: 'blur' }
  ],
  yearlyOrMonthlySavings: [
    { type: 'number', required: true, message: 'Yearly or monthly savings must be a number and is required', trigger: 'blur' },
    { type: 'number', min: 0, message: 'Yearly or monthly savings must be greater than or equal to 0', trigger: 'blur' }
  ],
  stockAnnualReturn: [
    { type: 'number', required: true, message: 'Stock annual return must be a number and is required', trigger: 'blur' },
    { type: 'number', min: 0.01, max: 200, message: 'Stock annual return must be between 0.01 and 200', trigger: 'blur' }
  ],
  stockCostRatio: [
    { type: 'number', required: true, message: 'Stock cost ratio must be a number and is required', trigger: 'blur' },
    { type: 'number', min: 0.01, max: 100, message: 'Stock cost ratio must be between 0.01 and 100', trigger: 'blur' }
  ],
  bondAnnualReturn: [
    { type: 'number', required: true, message: 'Bond annual return must be a number and is required', trigger: 'blur' },
    { type: 'number', min: 0.01, max: 200, message: 'Bond annual return must be between 0.01 and 200', trigger: 'blur' }
  ],
  bondCostRatio: [
    { type: 'number', required: true, message: 'Bond cost ratio must be a number and is required', trigger: 'blur' },
    { type: 'number', min: 0.01, max: 100, message: 'Bond cost ratio must be between 0.01 and 100', trigger: 'blur' }
  ],
  stockToBondRatio: [
    { type: 'number', required: true, message: 'Stock to bond ratio must be a number and is required', trigger: 'blur' },
    { type: 'number', min: 0, max: 100, message: 'Stock to bond ratio must be between 0 and 100', trigger: 'blur' }
  ],
  retirementDuration: [
    { type: 'number', required: true, message: 'Retirement duration must be a number and is required', trigger: 'blur' },
    { type: 'number', min: 1, max: 100, message: 'Retirement duration must be between 1 and 100 years', trigger: 'blur' }
  ],
  inflationRate: [
    { type: 'number', required: true, message: 'Inflation rate must be a number and is required', trigger: 'blur' },
    { type: 'number', min: 0.01, max: 100, message: 'Inflation rate must be between 0.01 and 100', trigger: 'blur' }
  ],
  withdrawalRate: [
    { type: 'number', required: true, message: 'Withdrawal rate must be a number and is required', trigger: 'blur' },
    { type: 'number', min: 0.01, max: 100, message: 'Withdrawal rate must be between 0.01 and 100', trigger: 'blur' }
  ]
})

const submitForm = async (formEl: FormInstance | undefined) => {
  if (!formEl) return
  await formEl.validate((valid, fields) => {
    if (valid) {
      resultsStore.calculateResults(userFinancialDataForm)
      if (userStore.getters.isLoggedIn) {
        updateUserFinancialData(userFinancialDataForm)
          .then(data => {
            console.log('Update successful!', data)
          })
          .catch(error => {
            console.error('Update failed!', error)
          })
      }
    } else {
      console.log('Error submit!', fields)
    }

  })
}

const resetForm = (formEl: FormInstance | undefined) => {
  if (!formEl) return
  formEl.resetFields()
}

// Fetch the initial data when the component is mounted, only if the user is logged in
onMounted(() => {
  if (userStore.getters.isLoggedIn) {
    getUserFinancialData()
      .then(data => {
        Object.assign(userFinancialDataForm, data)
        resultsStore.calculateResults(userFinancialDataForm)
      })
      .catch(error => {
        console.error('Failed to get user financial data', error)
      })
  }
  else {
    resultsStore.calculateResults(userFinancialDataForm)
  }

})

</script>

<style lang="scss">
.form-container {
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  gap: 20px;
  justify-content: center;
  margin: 0px 70px 0px 70px;
  height: 100%;
}

.data-form {
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  width: 60vw;
  max-width: 500px;
  min-width: 200px;
  align-items: flex-start;

  el-form-item {
    width: 100px;
  }

}

@media (max-width: 768px) {
  .data-form {
    width: 30%;
  }
}

.form-item {
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  align-items: flex-start;
  width: 100%;

  >div {
    width: 50%;
    min-width: 200px;
    padding: 5px 5px 5px 5px;
  }

  .el-form-item {
    display: flex;
    flex-direction: column;
  }
}

.form-descriptions {
  width: 100%;
  text-align: left;
  padding: 0px 20px 0px 20px;
  max-width: fit-content;

  p {
    padding: 6px 0px 6px 0px;
    margin: 8px 0px 8px 0px;
    height: 65px;
    width: 100%;
  }
}

.advanced-button {
  width: 100%;
}
</style>

<style lang="scss" scoped>
.frequency-button {
  width: 50%;

  &:focus {
    outline: none;
    box-shadow: none;
    background-color: #3d6b3c;
  }
}

.dark .frequency-button {
  &:focus {
    outline: none;
    box-shadow: none;
    background-color: #6d9b6d;
  }
}

.form-item-buttons {
  display: flex;
  gap: 0px;
  width: 100%;
  justify-content: center;

  .el-button {
    margin-left: 0px;
  }
}

.form-item-label {
  padding-bottom: 5px;
  max-width: 180px;
}

.ep-input-number {
  width: 100%;
}
.text-container {
  width: 500px;
  min-width: 300px;
  max-width: 500px;
  text-align: left;
  margin: 30px auto;
  padding: 0px 30px 30px 30px;
  max-width: fit-content;
  border-radius: 8px;

  .text {

    font-size: 18px;
    line-height: 1.7;
    padding: 6px 0;
    margin: 8px 0;
    width: 100%;
  }
}

</style>