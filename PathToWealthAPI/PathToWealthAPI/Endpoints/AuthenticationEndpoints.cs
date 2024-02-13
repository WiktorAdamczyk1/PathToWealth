using Microsoft.AspNetCore.Identity;
using FluentValidation;
using PathToWealthAPI.Services;
using static PathToWealthAPI.Data.Models;
using Microsoft.IdentityModel.Tokens;

namespace PathToWealthAPI.Endpoints
{
    public static class AuthenticationEndpoints
    {
        public static void MapAuthenticationEndpoints(this WebApplication app)
        {
            app.MapPost("/login", async (HttpContext httpContext, UserLogin userLogin, IPasswordHasher<User> passwordHasher, IUserService userService, ITokenService tokenService, IValidator<UserLogin> validator) =>
            {
                var validationResult = validator.Validate(userLogin);
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                User user = await userService.GetUser(userLogin.UsernameOrEmail);
                
                if (user == null || !userService.VerifyPassword(user, userLogin.Password, passwordHasher))
                {
                    return Results.Problem("Could not verify your current password", statusCode: 401);
                }

                var tokens = await tokenService.GenerateTokens(user);

                // Set the JWT and refresh token in HttpOnly cookies
                var jwtCookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(15)
                };
                var refreshTokenCookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(7) // Set the cookie to expire in 15 minutes
                };

                httpContext.Response.Cookies.Append("jwt", tokens.JwtToken, jwtCookieOptions);
                httpContext.Response.Cookies.Append("refreshToken", tokens.RefreshToken, refreshTokenCookieOptions);

                return Results.Ok(new { message = "Authentication successful", username = user.Username, email = user.Email });
            }).WithName("Login")
            .RequireRateLimiting("GeneralLimit")
            .RequireRateLimiting("IpLimit");


            app.MapPost("/register", async (UserRegistration registration, IPasswordHasher<User> passwordHasher, IValidator<UserRegistration> validator, IRegistrationService registrationService) =>
            {
                // Validate the registration model
                var validationResult = validator.Validate(registration);
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                try
                {
                    // Register the user
                    var user = await registrationService.RegisterUser(registration, passwordHasher);

                    // Exclude the password information from the response
                    var responseUser = new
                    {
                        user.UserId,
                        user.Username,
                        user.Email
                    };

                    return Results.Created($"/user/{user.UserId}", responseUser);
                }
                catch (Exception)
                {
                    return Results.Conflict("A conflict occurred while processing your registration request.");
                }

            }).WithName("Register")
            .RequireRateLimiting("GeneralLimit")
            .RequireRateLimiting("IpLimit");

            // Endpoint to refresh JWT token
            app.MapPost("/refresh-token", async (HttpContext httpContext, ITokenService tokenService) =>
            {
                // Read the refresh token from the request body or cookie
                var refreshToken = httpContext.Request.Cookies["refreshToken"];
                if (string.IsNullOrEmpty(refreshToken))
                {
                    refreshToken = httpContext.Request.Cookies["refreshToken"];
                }

                if (string.IsNullOrEmpty(refreshToken))
                {
                    return Results.BadRequest("Refresh token is required.");
                }

                try
                {
                    var tokens = await tokenService.RefreshJwtToken(refreshToken);

                    // Set the new JWT and refresh token in HttpOnly cookies
                    var jwtCookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = DateTime.UtcNow.AddMinutes(15)
                    };
                    var refreshTokenCookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTime.UtcNow.AddDays(7)
                    };

                    httpContext.Response.Cookies.Append("jwt", tokens.JwtToken, jwtCookieOptions);
                    httpContext.Response.Cookies.Append("refreshToken", tokens.RefreshToken, refreshTokenCookieOptions);

                    return Results.Ok(new { message = "Token refreshed successfully" });
                }
                catch (SecurityTokenException ex)
                {
                    return Results.Unauthorized();
                }
            }).WithName("RefreshToken")
            .RequireRateLimiting("GeneralLimit");

            // Endpoint to revoke refresh token
            app.MapPost("/revoke-token", async (HttpContext httpContext, ITokenService tokenService) =>
            {
                var refreshToken = httpContext.Request.Cookies["refreshToken"];
                if (string.IsNullOrEmpty(refreshToken))
                {
                    return Results.BadRequest("Refresh token is required.");
                }

                try
                {
                    await tokenService.RevokeRefreshToken(refreshToken);
                    return Results.NoContent();
                }
                catch (SecurityTokenException)
                {
                    return Results.BadRequest("The refresh token could not be revoked. Please try again.");
                }
            }).WithName("RevokeToken")
            .RequireRateLimiting("GeneralLimit")
            .RequireAuthorization();

            // Endpoint for logout
            app.MapPost("/logout", async (HttpContext httpContext, ITokenService tokenService, ILogger<Program> logger) =>
            {
                var refreshToken = httpContext.Request.Cookies["refreshToken"];
                bool tokenRevoked = false;

                if (!string.IsNullOrEmpty(refreshToken))
                {
                    try
                    {
                        await tokenService.RevokeRefreshToken(refreshToken);
                        tokenRevoked = true;
                    }
                    catch (SecurityTokenException ex)
                    {
                        // Log the failure to revoke the token
                        logger.LogError(ex, "Failed to revoke the refresh token: {RefreshToken}", refreshToken);
                    }
                }

                if (tokenRevoked)
                {
                    // Clear the JWT and refresh token cookies if they exist
                    httpContext.Response.Cookies.Delete("jwt");
                    httpContext.Response.Cookies.Delete("refreshToken");
                    logger.LogInformation("User logged out successfully.");
                    return Results.Ok("Logged out successfully.");
                }
                else
                {
                    return Results.BadRequest("Failed to revoke the refresh token. Logout was not completed.");
                }
            }).WithName("Logout")
            .RequireRateLimiting("GeneralLimit")
            .RequireAuthorization();
        }
    }
}