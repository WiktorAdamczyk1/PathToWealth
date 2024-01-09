using Microsoft.AspNetCore.Identity;
using FluentValidation;
using PathToWealthAPI.Data;
using PathToWealthAPI.Services;
using static PathToWealthAPI.Data.Models;

namespace PathToWealthAPI.Endpoints
{
    public static class AuthenticationEndpoints
    {
        public static void MapAuthenticationEndpoints(this WebApplication app)
        {
            app.MapPost("/login", async (ApplicationDbContext db, UserLogin userLogin, IPasswordHasher<User> passwordHasher, IUserService userService, ITokenService tokenService) =>
            {
                User user = await userService.GetUser(userLogin.UsernameOrEmail);

                if (user == null || !userService.VerifyPassword(user, userLogin.Password, passwordHasher))
                    return Results.Unauthorized();

                var token = tokenService.GenerateToken(user);
                return Results.Ok(new { token });
            }).WithName("Login")
            .WithOpenApi();

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
                catch (Exception ex)
                {
                    return Results.Conflict(ex.Message);
                }
            }).WithName("Register");
        }
    }
}