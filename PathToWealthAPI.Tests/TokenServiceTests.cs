using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using PathToWealthAPI.Services;
using Xunit;
using static PathToWealthAPI.Data.Models;

namespace PathToWealthAPI.Tests
{
    public class TokenServiceTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly TokenService _tokenService;

        public TokenServiceTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _tokenService = new TokenService(_mockConfiguration.Object);

            // Mock configuration for JWT settings
            _mockConfiguration.SetupGet(c => c["Jwt:Key"]).Returns("testKey1234567890abcdefghijklmnop");
            _mockConfiguration.SetupGet(c => c["Jwt:Issuer"]).Returns("TestIssuer");
            _mockConfiguration.SetupGet(c => c["Jwt:Audience"]).Returns("TestAudience");
        }

        [Fact]
        public void GenerateToken_ReturnsValidToken_WhenUserIsValid()
        {
            // Arrange
            var user = new User { UserId = 1, Username = "testUser" };

            // Act
            var token = _tokenService.GenerateToken(user);

            // Assert
            Assert.NotEmpty(token);

            // Additional assertions to verify the structure and validity of the token
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("testKey1234567890abcdefghijklmnop")),
                ValidateIssuer = true,
                ValidIssuer = "TestIssuer",
                ValidateAudience = true,
                ValidAudience = "TestAudience",
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            SecurityToken validatedToken;
            var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;

            Assert.Equal(user.Username, principal.Identity.Name);
            Assert.Equal(user.UserId.ToString(), principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}
