using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PathToWealthAPI.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static PathToWealthAPI.Data.Models;

namespace PathToWealthAPI.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IApplicationDbContext _dbContext;

        public TokenService(IConfiguration configuration, IApplicationDbContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        public async Task<TokenResponse> GenerateTokens(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var jwtToken = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            // Store the refresh token in the database
            var refreshTokenEntity = new RefreshToken
            {
                UserId = user.UserId,
                Token = refreshToken,
                Expires = DateTime.UtcNow.AddDays(7), // Set the expiry date
                Created = DateTime.UtcNow
            };

            _dbContext.RefreshToken.Add(refreshTokenEntity);
            await _dbContext.SaveChangesAsync();

            return new TokenResponse
            {
                JwtToken = jwtToken,
                RefreshToken = refreshToken
            };
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddMinutes(1), // Shorter expiry for JWT
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public async Task<TokenResponse> RefreshJwtToken(string refreshToken)
        {
            var refreshTokenEntity = await _dbContext.RefreshToken.FirstOrDefaultAsync(rt => rt.Token == refreshToken 
                                                            && rt.Expires > DateTime.UtcNow && rt.Revoked == null);
            if (refreshTokenEntity == null)
            {
                throw new SecurityTokenException("Invalid refresh token");
            }

            var user = await _dbContext.User.FindAsync(refreshTokenEntity.UserId);
            if (user == null)
            {
                throw new InvalidOperationException("User for the provided refresh token does not exist");
            }

            // Invalidate the old refresh token
            refreshTokenEntity.Revoked = DateTime.UtcNow;

            // Generate a new refresh token and a new JWT
            var newRefreshToken = GenerateRefreshToken();
            var newJwtToken = GenerateJwtToken(user);

            // Save the new refresh token to the database
            var newRefreshTokenEntity = new RefreshToken
            {
                UserId = user.UserId,
                Token = newRefreshToken,
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow
            };

            _dbContext.RefreshToken.Add(newRefreshTokenEntity);
            await _dbContext.SaveChangesAsync();

            return new TokenResponse
            {
                JwtToken = newJwtToken,
                RefreshToken = newRefreshToken
            };
        }

        public async Task RevokeRefreshToken(string refreshToken)
        {
            var refreshTokenEntity = await _dbContext.RefreshToken.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
            if (refreshTokenEntity == null || refreshTokenEntity.Expires <= DateTime.UtcNow || refreshTokenEntity.Revoked != null)
            {
                throw new SecurityTokenException("Refresh token is not valid or already revoked");
            }

            refreshTokenEntity.Revoked = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
        }
    }

    public class TokenResponse
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
