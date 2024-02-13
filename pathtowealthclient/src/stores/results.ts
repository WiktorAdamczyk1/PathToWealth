import { reactive } from 'vue'
import { calculateInvestmentData, calculateWithdrawalData } from '../calculations'
import { UserFinancialData } from '../requests'

const state = reactive({
    totalMoneyAvailable: [] as number[],
    totalContributions: [] as number[],
    totalInterestEarned: [] as number[],
    totalCosts: [] as number[],

    withdrawalAmounts: [] as number[],
    remainingAmounts: [] as number[],
    annualReturns: [] as number[],
    procuredCosts: [] as number[],

    startInvestmentYear: 0,
    startWithdrawalYear: 0,
    retirementDuration: 0
})

const actions = {
    calculateResults(data: UserFinancialData) {
        const investmentResults = calculateInvestmentData(data)
        state.totalMoneyAvailable  = investmentResults.totalMoneyAvailable
        state.totalContributions = investmentResults.totalContributions
        state.totalInterestEarned = investmentResults.totalInterestEarned
        state.totalCosts = investmentResults.totalCosts
        
        const withdrawalResults = calculateWithdrawalData(state.totalMoneyAvailable[state.totalMoneyAvailable.length - 1], data)
        state.withdrawalAmounts = withdrawalResults.withdrawalAmounts
        state.remainingAmounts = withdrawalResults.remainingAmounts
        state.annualReturns = withdrawalResults.annualReturns
        state.procuredCosts = withdrawalResults.procuredCosts

        state.startInvestmentYear = data.startInvestmentYear
        state.startWithdrawalYear = data.startWithdrawalYear
        state.retirementDuration = data.retirementDuration
    }
}

export default { state, ...actions }