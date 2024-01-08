using System.ComponentModel.DataAnnotations;

namespace PathToWealthAPI.Data
{
    public class Models
    {

        public class User
        {
            public int UserId { get; set; }
            [Required]
            public string Username { get; set; }
            [Required]
            public string PasswordHash { get; set; }
            [Required]
            public string Email { get; set; }
        }

        public class JwtToken
        {
            public int TokenId { get; set; }
            public int UserId { get; set; }
            [Required]
            public string Token { get; set; }
            [Required]
            public DateTime ExpiryDate { get; set; }
        }

        public class UserFinancialData
        {
            public int DataId { get; set; }
            public int UserId { get; set; }
            public int Age { get; set; }
            public decimal InitialInvestment { get; set; }
            public decimal AnnualIncome { get; set; }
            public decimal InvestmentPercentage { get; set; }
            public int RetirementAge { get; set; }
            public decimal AnnualRetirementIncome { get; set; }
            public decimal FundBondRatio { get; set; }
            public string PreferredFundsBonds { get; set; }
            public int HistoricalInvestmentYear { get; set; }
            public decimal FutureSavingsGoal { get; set; }
            public int WithdrawalAge { get; set; }
            public decimal AnnualWithdrawalAmount { get; set; }
        }

    }
}
