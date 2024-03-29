﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
            public List<RefreshToken> RefreshTokens { get; set; } // Allows multiple devices to have separate tokens
            public UserFinancialData UserFinancialData { get; set; }
        }

        public class RefreshToken
        {
            [Key]
            public int RefreshTokenId { get; set; }
            public int UserId { get; set; }
            public string Token { get; set; }
            public DateTime Expires { get; set; }
            public DateTime Created { get; set; }
            public DateTime? Revoked { get; set; }
            public User User { get; set; }
        }

        public class UserFinancialData
        {
            [Key]
            [JsonIgnore]
            public int DataId { get; set; }
            [JsonIgnore]
            public int UserId { get; set; }
            public decimal InitialInvestment { get; set; } = 10000;
            public int StartInvestmentYear { get; set; } = DateTime.UtcNow.Year;
            public int StartWithdrawalYear { get; set; } = DateTime.UtcNow.Year + 30;
            public bool IsInvestmentMonthly { get; set; } = false; // False indicates yearly
            public decimal YearlyOrMonthlySavings { get; set; } = 12000;
            public decimal StockAnnualReturn { get; set; } = 7.90M;
            public decimal StockCostRatio { get; set; } = 0.04M;
            public decimal BondAnnualReturn { get; set; } = 3.30M;
            public decimal BondCostRatio { get; set; } = 0.05M;
            public decimal StockToBondRatio { get; set; } = 100; // 100 indicates investment only in stocks fund
            public int RetirementDuration { get; set; } = 30;
            public decimal InflationRate { get; set; } = 2.50M;
            public decimal WithdrawalRate { get; set; } = 4.00M;
            [JsonIgnore]
            public User User { get; set; }
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

        public class UserPasswordChange
        {
            public string CurrentPassword { get; set; }
            public string NewPassword { get; set; }
        }

    }
}