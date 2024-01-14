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
            if (usernameOrEmail == null)
            {
                throw new ArgumentNullException(nameof(usernameOrEmail));
            }

            if (usernameOrEmail.Contains("@"))
            {
                return await _db.User.FirstOrDefaultAsync(u => u.Email == usernameOrEmail);
            }
            else
            {
                return await _db.User.FirstOrDefaultAsync(u => u.Username == usernameOrEmail);
            }
        }

        public async Task<User> GetUser(int userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            return await _db.User.FirstOrDefaultAsync(u => u.UserId == userId);
            
        }

        public bool VerifyPassword(User user, string password, IPasswordHasher<User> passwordHasher)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            var verificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return verificationResult != PasswordVerificationResult.Failed;
        }


        public async Task DeleteUser(int userId)
        {
            var user = await _db.User.FindAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            _db.User.Remove(user);
            await _db.SaveChangesAsync();
        }

        public async Task UpdatePassword(int userId, string newPassword, IPasswordHasher<User> passwordHasher)
        {
            var user = await _db.User.FindAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            user.PasswordHash = passwordHasher.HashPassword(user, newPassword);
            _db.User.Update(user);
            await _db.SaveChangesAsync();
        }


    }
}
