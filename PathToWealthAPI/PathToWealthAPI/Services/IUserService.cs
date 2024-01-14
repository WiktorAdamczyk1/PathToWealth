using Microsoft.AspNetCore.Identity;
using static PathToWealthAPI.Data.Models;

namespace PathToWealthAPI.Services
{
    public interface IUserService
    {
        Task DeleteUser(int userId);
        Task<User> GetUser(string usernameOrEmail);
        Task<User> GetUser(int userId);
        Task UpdatePassword(int userId, string newPassword, IPasswordHasher<User> passwordHasher);
        bool VerifyPassword(User user, string password, IPasswordHasher<User> passwordHasher);
    }
}
