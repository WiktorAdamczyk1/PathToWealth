using Microsoft.EntityFrameworkCore;
using PathToWealthAPI.Data;
using System.Security.Claims;
using static PathToWealthAPI.Data.Models;

namespace PathToWealthAPI.Services
{

    public class UserFinancialDataService : IUserFinancialDataService
    {
        private readonly IApplicationDbContext _db;

        public UserFinancialDataService(IApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IResult> GetUserFinancialData(HttpContext httpContext)
        {
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Results.Unauthorized();
            }
            var userId = int.Parse(userIdClaim.Value);

            var userFinancialData = await _db.UserFinancialData.Where(u => u.UserId == userId).ToListAsync();
            if (userFinancialData == null || !userFinancialData.Any())
            {
                return Results.NotFound();
            }

            return Results.Ok(userFinancialData);
        }

        public async Task<IResult> UpdateUserFinancialData(HttpContext httpContext, UserFinancialData updatedData)
        {
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Results.Unauthorized();
            }
            var userId = int.Parse(userIdClaim.Value);

            var userFinancialData = await _db.UserFinancialData.FirstOrDefaultAsync(u => u.UserId == userId);
            if (userFinancialData == null)
            {
                return Results.NotFound();
            }

            userFinancialData.Age = updatedData.Age;
            userFinancialData.InitialInvestment = updatedData.InitialInvestment;
            userFinancialData.AnnualIncome = updatedData.AnnualIncome;
            userFinancialData.InvestmentPercentage = updatedData.InvestmentPercentage;
            userFinancialData.RetirementAge = updatedData.RetirementAge;
            userFinancialData.AnnualRetirementIncome = updatedData.AnnualRetirementIncome;
            userFinancialData.FundBondRatio = updatedData.FundBondRatio;
            userFinancialData.PreferredFunds = updatedData.PreferredFunds;
            userFinancialData.PreferredBonds = updatedData.PreferredBonds;
            userFinancialData.HistoricalInvestmentYear = updatedData.HistoricalInvestmentYear;
            userFinancialData.FutureSavingsGoal = updatedData.FutureSavingsGoal;
            userFinancialData.WithdrawalAge = updatedData.WithdrawalAge;
            userFinancialData.AnnualWithdrawalAmount = updatedData.AnnualWithdrawalAmount;

            await _db.SaveChangesAsync();
            return Results.NoContent();
        }

        public async Task InsertUserFinancialData(int userId, UserFinancialData userFinancialData)
        {
            userFinancialData.UserId = userId;
            _db.UserFinancialData.Add(userFinancialData);
            await _db.SaveChangesAsync();
        }

    }
}