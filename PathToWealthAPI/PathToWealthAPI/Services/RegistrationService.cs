using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PathToWealthAPI.Data;
using static PathToWealthAPI.Data.Models;

namespace PathToWealthAPI.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly ApplicationDbContext _db;

        public RegistrationService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<User> RegisterUser(UserRegistration registration, IPasswordHasher<User> passwordHasher)
        {
            // Check if a user with the same username or email already exists
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

            // Add the new user to the database
            _db.User.Add(user);
            await _db.SaveChangesAsync();

            return user;
        }
    }
}
