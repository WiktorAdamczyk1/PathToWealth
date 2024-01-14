using static PathToWealthAPI.Data.Models;

namespace PathToWealthAPI.Services
{
    public interface IUserFinancialDataService
    {
        Task<IResult> GetUserFinancialData(HttpContext httpContext);
        Task<IResult> UpdateUserFinancialData(HttpContext httpContext, UserFinancialDataUpdate updatedData);
        Task InsertUserFinancialData(int userId, UserFinancialData userFinancialData);
    }
}
