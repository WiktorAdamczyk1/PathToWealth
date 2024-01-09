using Microsoft.AspNetCore.Identity;
using static PathToWealthAPI.Data.Models;

namespace PathToWealthAPI.Services
{
    public interface IUserService
    {
        Task<User> GetUser(string usernameOrEmail);
        bool VerifyPassword(User user, string password, IPasswordHasher<User> passwordHasher);
    }
}
