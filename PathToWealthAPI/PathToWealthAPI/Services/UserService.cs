using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PathToWealthAPI.Data;
using static PathToWealthAPI.Data.Models;

namespace PathToWealthAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IApplicationDbContext _db;

        public UserService(IApplicationDbContext db, IPasswordHasher<User> @object)
        {
            _db = db;
        }

        public async Task<User> GetUser(string usernameOrEmail)
        {
            if (usernameOrEmail.Contains("@"))
            {
                return await _db.User.FirstOrDefaultAsync(u => u.Email == usernameOrEmail);
            }
            else
            {
                return await _db.User.FirstOrDefaultAsync(u => u.Username == usernameOrEmail);
            }
        }

        public bool VerifyPassword(User user, string password, IPasswordHasher<User> passwordHasher)
        {
            var verificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return verificationResult != PasswordVerificationResult.Failed;
        }
    }
}
