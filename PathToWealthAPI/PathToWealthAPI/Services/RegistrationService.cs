using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PathToWealthAPI.Data;
using static PathToWealthAPI.Data.Models;

namespace PathToWealthAPI.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IApplicationDbContext _db;

        public RegistrationService(IApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<User> RegisterUser(UserRegistration registration, IPasswordHasher<User> passwordHasher, UserFinancialData financialData = null)
        {
            var existingUser = await _db.User.FirstOrDefaultAsync(u => u.Username == registration.Username || u.Email == registration.Email);
            if (existingUser != null)
            {
                throw new Exception("Username or email already exists");
            }

            // Create a new user entity and hash the password
            var user = new User
            {
                Username = registration.Username,
                Email = registration.Email,
                PasswordHash = passwordHasher.HashPassword(null, registration.Password)
            };

            _db.User.Add(user);
            await _db.SaveChangesAsync();

            // If financial data is provided, use it; otherwise, create a new object with null values
            var userFinancialData = financialData ?? new UserFinancialData { UserId = user.UserId };
            _db.UserFinancialData.Add(userFinancialData);
            await _db.SaveChangesAsync();

            return user;
        }

    }
}