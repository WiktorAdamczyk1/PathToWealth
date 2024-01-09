using System.ComponentModel.DataAnnotations;

namespace PathToWealthAPI.Data
{
    public class Models
    {

        public class User
        {
            [Key]
            public int UserId { get; set; }
            public string Username { get; set; }
            public string PasswordHash { get; set; }
            public string Email { get; set; }
        }

        public class JwtToken
        {
            [Key]
            public int TokenId { get; set; }
            public int UserId { get; set; }
            public string Token { get; set; }
            public DateTime ExpiryDate { get; set; }
        }

        public class UserFinancialData
        {
            [Key]
            public int DataId { get; set; }
            public int UserId { get; set; }
            public int Age { get; set; }
            public decimal InitialInvestment { get; set; }
            public decimal AnnualIncome { get; set; }
            public decimal InvestmentPercentage { get; set; }
            public int RetirementAge { get; set; }
            public decimal AnnualRetirementIncome { get; set; }
            public decimal FundBondRatio { get; set; }
            public string PreferredFunds{ get; set; }
            public string PreferredBonds { get; set; }
            public int HistoricalInvestmentYear { get; set; }
            public decimal FutureSavingsGoal { get; set; }
            public int WithdrawalAge { get; set; }
            public decimal AnnualWithdrawalAmount { get; set; }
        }

        public class UserLogin
        {
            public string UsernameOrEmail { get; set; }
            public string Password { get; set; }
        }

        public class UserRegistration
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string PasswordConfirmation { get; set; }
        }


    }
}
