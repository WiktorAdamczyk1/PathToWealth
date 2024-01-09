using static PathToWealthAPI.Data.Models;

namespace PathToWealthAPI.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
