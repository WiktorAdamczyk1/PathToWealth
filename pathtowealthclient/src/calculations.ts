import { UserFinancialData } from './requests';

function calculateTotalContributions(data: UserFinancialData): number[] {
    const {
        initialInvestment,
        startInvestmentYear,
        startWithdrawalYear,
        isInvestmentMonthly,
        yearlyOrMonthlySavings
    } = data;

    const totalContributions: number[] = [];

    let currentYear = startInvestmentYear;
    let amountPutIn = initialInvestment;

    while (currentYear < startWithdrawalYear) {
        totalContributions.push(amountPutIn);
        currentYear++;
        amountPutIn += isInvestmentMonthly ? yearlyOrMonthlySavings * 12 : yearlyOrMonthlySavings;
    }

    return totalContributions;
}
  
function calculateTotalInterestWithCosts(data: UserFinancialData, totalContributions: number[]): { totalInterestEarned: number[], totalCosts: number[] } {
    const {
        stockAnnualReturn,
        bondAnnualReturn,
        stockToBondRatio,
        stockCostRatio,
        bondCostRatio
    } = data;

    const totalInterestEarned: number[] = [];
    const totalCosts: number[] = [];

    let currentContribution = 0;
    let currentInterest = 0;
    let currentCost = 0;

    for (let i = 0; i < totalContributions.length; i++) {
        currentContribution = totalContributions[i];
        currentInterest += (currentContribution + currentInterest - currentCost) * (stockToBondRatio / 100) * (stockAnnualReturn / 100) + 
        (currentContribution + currentInterest - currentCost) * ((100 - stockToBondRatio) / 100) * (bondAnnualReturn / 100);
        currentCost += (currentContribution + currentInterest - currentCost) * (stockToBondRatio / 100) * (stockCostRatio / 100) +
        (currentContribution + currentInterest - currentCost) * ((100 - stockToBondRatio) / 100) * (bondCostRatio / 100);
        totalInterestEarned.push(currentInterest);
        totalCosts.push(currentCost);
    }

    return { totalInterestEarned, totalCosts };
}


function calculateMoneyAvailable(data: UserFinancialData, totalContributions: number[], totalInterestEarned: number[], totalCosts: number[]): number[] {
    const {
        startInvestmentYear,
        startWithdrawalYear
    } = data;

    const totalMoneyAvailable: number[] = [];
    let currentYear = startInvestmentYear;
    let amountAvailable = 0;

    while (currentYear < startWithdrawalYear) {
        const amountPutIn = totalContributions[currentYear - startInvestmentYear];
        const interestEarned = totalInterestEarned[currentYear - startInvestmentYear];
        const totalCost = totalCosts[currentYear - startInvestmentYear];

        amountAvailable = amountPutIn + interestEarned - totalCost;
        totalMoneyAvailable.push(amountAvailable);

        currentYear++;
    }

    return totalMoneyAvailable;
}

  
  export function calculateInvestmentData(data: UserFinancialData): {
    totalMoneyAvailable: number[],
    totalContributions: number[],
    totalInterestEarned: number[],
    totalCosts: number[]
  } {
    const {
        initialInvestment,
        startInvestmentYear,
        startWithdrawalYear,
        isInvestmentMonthly,
        yearlyOrMonthlySavings,
        stockAnnualReturn,
        stockCostRatio,
        bondAnnualReturn,
        bondCostRatio,
        stockToBondRatio,
        inflationRate,
        withdrawalRate
    } = data;
  
    const totalContributions = calculateTotalContributions(data);
    const { totalInterestEarned, totalCosts } = calculateTotalInterestWithCosts(data, totalContributions);
    const totalMoneyAvailable = calculateMoneyAvailable(data, totalContributions, totalInterestEarned, totalCosts);
  
    return {
      totalMoneyAvailable,
      totalContributions,
      totalInterestEarned,
      totalCosts
    };
  }

export function calculateWithdrawalData(finalAmount: number, data: UserFinancialData): {withdrawalAmounts: number[], remainingAmounts: number[], annualReturns: number[], procuredCosts: number[]} {
    const {
        retirementDuration,
        bondAnnualReturn,
        bondCostRatio,
        inflationRate,
        withdrawalRate
    } = data;

    const withdrawalAmounts: number[] = [];
    const remainingAmounts: number[] = [];
    const annualReturns: number[] = [];
    const procuredCosts: number[] = [];
    
    let withdrawalAmount = finalAmount * (withdrawalRate / 100);
    let remainingAmount = finalAmount;
    
    for (let i = 0; i < retirementDuration; i++) {
        withdrawalAmount *= 1 + (inflationRate / 100);
        if (withdrawalAmount > remainingAmount) {
            withdrawalAmount = remainingAmount; // Withdraw the remaining amount
        }
        withdrawalAmounts.push(withdrawalAmount);

        remainingAmount -= withdrawalAmount; // Subtract the withdrawal amount from the remaining amount
        let annualReturn = remainingAmount * (bondAnnualReturn / 100);
        let annualCost = remainingAmount * (bondCostRatio / 100);
        remainingAmount += annualReturn - annualCost; // Add the bond return to the remaining amount
        remainingAmounts.push(remainingAmount);
        annualReturns.push(annualReturn);
        procuredCosts.push(annualCost);
    }

    return {withdrawalAmounts, remainingAmounts, annualReturns, procuredCosts};
}
