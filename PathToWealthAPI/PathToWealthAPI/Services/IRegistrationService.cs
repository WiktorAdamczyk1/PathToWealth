using Microsoft.AspNetCore.Identity;
using static PathToWealthAPI.Data.Models;

namespace PathToWealthAPI.Services
{
    public interface IRegistrationService
    {
        Task<User> RegisterUser(UserRegistration registration, IPasswordHasher<User> passwordHasher, UserFinancialData financialData = null);
    }
}