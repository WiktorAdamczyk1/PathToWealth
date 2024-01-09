using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PathToWealthAPI;
using PathToWealthAPI.Data;
using PathToWealthAPI.Extensions;
using PathToWealthAPI.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static PathToWealthAPI.Data.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PathToWealthDbConnn")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddFluentValidationServices();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/login", async (ApplicationDbContext db, Models.UserLogin userLogin, IPasswordHasher<User> passwordHasher, IUserService userService, ITokenService tokenService) =>
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


app.Run();
