using Microsoft.AspNetCore.Identity;
using FluentValidation;
using PathToWealthAPI.Services;
using static PathToWealthAPI.Data.Models;
using System.Security.Claims;

namespace PathToWealthAPI.Endpoints
{
    public static class UserAccountEndpoints
    {
        public static void MapUserAccountEndpoints(this WebApplication app)
        {
            app.MapDelete("/delete-account", async (HttpContext httpContext, IUserService userService) =>
            {
                var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Results.Unauthorized();
                }

                var userId = int.Parse(userIdClaim.Value);
                await userService.DeleteUser(userId);

                return Results.Ok(new { message = "Account deleted successfully" });
            }).WithName("DeleteAccount")
            .RequireRateLimiting("GeneralLimit")
            .RequireRateLimiting("IpLimit")
            .RequireAuthorization();

            app.MapPost("/change-password", async (HttpContext httpContext, UserPasswordChange userPasswordChange, IPasswordHasher<User> passwordHasher, IUserService userService) =>
            {
                // Validate the model
                var validator = new ChangePasswordValidator();
                var validationResult = validator.Validate(userPasswordChange);
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                // Get the user ID from the JWT token
                var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Results.Problem("Could not retreive the userID from JWT token", statusCode: 401);
                }
                var userId = int.Parse(userIdClaim.Value);
                Console.WriteLine(userId);
                // Verify the current password
                User user = await userService.GetUser(userId);
                if (user == null || !userService.VerifyPassword(user, userPasswordChange.CurrentPassword, passwordHasher))
                {
                    return Results.Problem("Could not verify your current password", statusCode: 401);
                }

                // Update the password
                await userService.UpdatePassword(userId, userPasswordChange.NewPassword, passwordHasher);

                return Results.Ok(new { message = "Password changed successfully" });
            }).WithName("ChangePassword")
            .RequireRateLimiting("GeneralLimit")
            .RequireAuthorization();
        }
    }
}
