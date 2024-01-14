using static PathToWealthAPI.Data.Models;

namespace PathToWealthAPI.Services
{
    public interface ITokenService
    {
        Task<TokenResponse> GenerateTokens(User user);
        Task<TokenResponse> RefreshJwtToken(string refreshToken);
        Task RevokeRefreshToken(string refreshToken);
    }
}
