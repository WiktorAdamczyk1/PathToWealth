using static PathToWealthAPI.Data.Models;

namespace PathToWealthAPI.Services
{
    public interface IUserFinancialDataService
    {
        Task<IResult> GetUserFinancialData(HttpContext httpContext);
        Task<IResult> UpdateUserFinancialData(HttpContext httpContext, UserFinancialData updatedData);
        Task InsertUserFinancialData(int userId, UserFinancialData userFinancialData);
    }
}
