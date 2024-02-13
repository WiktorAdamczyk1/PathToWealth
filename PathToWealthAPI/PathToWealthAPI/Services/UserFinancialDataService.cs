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

            var userFinancialData = await _db.UserFinancialData.FirstOrDefaultAsync(u => u.UserId == userId);
            if (userFinancialData == null)
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

            // Update the properties based on the new model structure
            userFinancialData.InitialInvestment = updatedData.InitialInvestment;
            userFinancialData.StartInvestmentYear = updatedData.StartInvestmentYear;
            userFinancialData.StartWithdrawalYear = updatedData.StartWithdrawalYear;
            userFinancialData.IsInvestmentMonthly = updatedData.IsInvestmentMonthly;
            userFinancialData.YearlyOrMonthlySavings = updatedData.YearlyOrMonthlySavings;
            userFinancialData.StockAnnualReturn = updatedData.StockAnnualReturn;
            userFinancialData.StockCostRatio = updatedData.StockCostRatio;
            userFinancialData.BondAnnualReturn = updatedData.BondAnnualReturn;
            userFinancialData.BondCostRatio = updatedData.BondCostRatio;
            userFinancialData.StockToBondRatio = updatedData.StockToBondRatio;
            userFinancialData.RetirementDuration = updatedData.RetirementDuration;
            userFinancialData.InflationRate = updatedData.InflationRate;
            userFinancialData.WithdrawalRate = updatedData.WithdrawalRate;

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