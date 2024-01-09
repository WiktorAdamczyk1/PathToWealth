using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PathToWealthAPI.Data;
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/login", async (ApplicationDbContext db, Models.UserLogin userLogin, IPasswordHasher<User> passwordHasher) =>
{
    // This should be a method that checks the user's credentials
    var user = await db.User.FirstOrDefaultAsync(u => u.Username == userLogin.Username);

    if (user == null)
        return Results.Unauthorized();

    // Verify the provided password with the stored hashed password
    var verificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, userLogin.Password);

    if (verificationResult == PasswordVerificationResult.Failed)
        return Results.Unauthorized();

    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, user.Username.ToString())
            // Add other claims as needed
        }),
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };
    var token = tokenHandler.CreateToken(tokenDescriptor);
    return Results.Ok(new { token = tokenHandler.WriteToken(token) });
}).WithName("Login")
.WithOpenApi();


app.MapPost("/register", async (ApplicationDbContext db, User user, IPasswordHasher<User> passwordHasher) =>
{
    // Check if a user with the same username already exists
    var existingUser = await db.User.FirstOrDefaultAsync(u => u.Username == user.Username);
    if (existingUser != null)
        return Results.Conflict("Username already exists");

    // Hash the password before storing it
    user.PasswordHash = passwordHasher.HashPassword(user, user.PasswordHash);

    // Add the new user to the database
    db.User.Add(user);
    await db.SaveChangesAsync();

    return Results.Created($"/user/{user.UserId}", user);
}).WithName("Register");


app.Run();
