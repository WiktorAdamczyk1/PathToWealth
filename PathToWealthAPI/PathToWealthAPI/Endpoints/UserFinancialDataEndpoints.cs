using PathToWealthAPI.Services;
using FluentValidation;
using static PathToWealthAPI.Data.Models;

namespace PathToWealthAPI.Endpoints
{
    public static class UserFinancialDataEndpoints
    {
        public static void MapUserFinancialDataEndpoints(this WebApplication app)
        {
            app.MapGet("/userfinancialdata", async (HttpContext httpContext, IUserFinancialDataService userFinancialDataService) =>
            {
                return await userFinancialDataService.GetUserFinancialData(httpContext);
            })
            .RequireAuthorization();

            app.MapPut("/userfinancialdata", async (HttpContext httpContext, UserFinancialData updatedData, IUserFinancialDataService userFinancialDataService, IValidator<UserFinancialData> validator) =>
            {
                var validationResult = validator.Validate(updatedData);
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }
                return await userFinancialDataService.UpdateUserFinancialData(httpContext, updatedData);
            })
.           RequireAuthorization();

        }
    }
}