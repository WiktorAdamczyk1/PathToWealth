using System;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MockQueryable.Moq;
using Moq;
using PathToWealthAPI.Data;
using PathToWealthAPI.Services;
using Xunit;
using static PathToWealthAPI.Data.Models;

namespace PathToWealthAPI.Tests
{
    public class TokenServiceTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IApplicationDbContext> _mockDbContext;
        private readonly TokenService _tokenService;

        public TokenServiceTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockDbContext = new Mock<IApplicationDbContext>();

            // Mock configuration for JWT settings
            _mockConfiguration.SetupGet(c => c["Jwt:Key"]).Returns("testKey1234567890abcdefghijklmnop");
            _mockConfiguration.SetupGet(c => c["Jwt:Issuer"]).Returns("TestIssuer");
            _mockConfiguration.SetupGet(c => c["Jwt:Audience"]).Returns("TestAudience");

            _tokenService = new TokenService(_mockConfiguration.Object, _mockDbContext.Object);
        }

        [Fact]
        public async Task GenerateTokens_ShouldGenerateTokensAndStoreRefreshToken()
        {
            // Arrange
            var user = new User { UserId = 1, Username = "testUser" };
            var mockSet = new Mock<DbSet<RefreshToken>>();
            _mockDbContext.Setup(db => db.RefreshToken).Returns(mockSet.Object);

            // Act
            var result = await _tokenService.GenerateTokens(user);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.JwtToken);
            Assert.NotNull(result.RefreshToken);
            mockSet.Verify(m => m.Add(It.IsAny<RefreshToken>()), Times.Once());
            _mockDbContext.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());

        }

        [Fact]
        public async Task GenerateTokens_ShouldHandleNullUserException()
        {
            // Arrange
            User user = null;

            // Act
            var exception = await Record.ExceptionAsync(() => _tokenService.GenerateTokens(user));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public async Task RefreshJwtToken_ShouldRefreshTokenSuccessfully()
        {
            // Arrange
            var user = new User { UserId = 1, Username = "testUser" };
            var refreshToken = new RefreshToken { UserId = user.UserId, Token = "oldRefreshToken", Expires = DateTime.UtcNow.AddDays(7) };
            var refreshTokens = new List<RefreshToken> { refreshToken }.AsQueryable();

            var mockSet = refreshTokens.BuildMockDbSet();
            _mockDbContext.Setup(db => db.RefreshToken).Returns(mockSet.Object);
            _mockDbContext.Setup(db => db.User.FindAsync(user.UserId)).ReturnsAsync(user);

            // Act
            var result = await _tokenService.RefreshJwtToken(refreshToken.Token);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.JwtToken);
            Assert.NotNull(result.RefreshToken);
            Assert.NotEqual(refreshToken.Token, result.RefreshToken);
        }

        [Fact]
        public async Task RefreshJwtToken_ShouldInvalidateOldRefreshToken()
        {
            // Arrange
            var user = new User { UserId = 1, Username = "testUser" };
            var refreshToken = new RefreshToken { UserId = user.UserId, Token = "oldRefreshToken", Expires = DateTime.UtcNow.AddDays(7) };
            var refreshTokens = new List<RefreshToken> { refreshToken }.AsQueryable();

            var mockSet = refreshTokens.BuildMockDbSet();
            _mockDbContext.Setup(db => db.RefreshToken).Returns(mockSet.Object);
            _mockDbContext.Setup(db => db.User.FindAsync(user.UserId)).ReturnsAsync(user);

            // Act
            await _tokenService.RefreshJwtToken(refreshToken.Token);

            // Assert
            Assert.NotNull(refreshToken.Revoked);
        }

        [Fact]
        public async Task RefreshJwtToken_ShouldThrowExceptionForInvalidToken()
        {
            // Arrange
            var invalidToken = "invalidToken";
            var refreshTokens = new List<RefreshToken>().AsQueryable(); // No tokens in the list

            var mockSet = refreshTokens.BuildMockDbSet();
            _mockDbContext.Setup(db => db.RefreshToken).Returns(mockSet.Object);

            // Act
            var exception = await Record.ExceptionAsync(() => _tokenService.RefreshJwtToken(invalidToken));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<SecurityTokenException>(exception);
        }

        [Fact]
        public async Task RefreshJwtToken_ShouldThrowExceptionForNonExistentUser()
        {
            // Arrange
            var refreshToken = new RefreshToken { UserId = 1, Token = "oldRefreshToken", Expires = DateTime.UtcNow.AddDays(7) };
            var refreshTokens = new List<RefreshToken> { refreshToken }.AsQueryable();

            var mockSet = refreshTokens.BuildMockDbSet();
            _mockDbContext.Setup(db => db.RefreshToken).Returns(mockSet.Object);
            _mockDbContext.Setup(db => db.User.FindAsync(refreshToken.UserId)).ReturnsAsync((User)null);

            // Act
            var exception = await Record.ExceptionAsync(() => _tokenService.RefreshJwtToken(refreshToken.Token));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
        }

        [Fact]
        public async Task RevokeRefreshToken_ShouldRevokeTokenSuccessfully()
        {
            // Arrange
            var refreshToken = new RefreshToken { UserId = 1, Token = "validToken", Expires = DateTime.UtcNow.AddDays(7) };
            var refreshTokens = new List<RefreshToken> { refreshToken }.AsQueryable();

            var mockSet = refreshTokens.BuildMockDbSet();
            _mockDbContext.Setup(db => db.RefreshToken).Returns(mockSet.Object);

            // Act
            await _tokenService.RevokeRefreshToken(refreshToken.Token);

            // Assert
            Assert.NotNull(refreshToken.Revoked);
        }

        [Fact]
        public async Task RevokeRefreshToken_ShouldThrowExceptionForInvalidToken()
        {
            // Arrange
            var invalidToken = "invalidToken";
            var refreshTokens = new List<RefreshToken>().AsQueryable(); // No tokens in the list

            var mockSet = refreshTokens.BuildMockDbSet();
            _mockDbContext.Setup(db => db.RefreshToken).Returns(mockSet.Object);

            // Act
            var exception = await Record.ExceptionAsync(() => _tokenService.RevokeRefreshToken(invalidToken));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<SecurityTokenException>(exception);
        }

        [Fact]
        public async Task RevokeRefreshToken_ShouldThrowExceptionForNonExistentToken()
        {
            // Arrange
            var nonExistentToken = "nonExistentToken";
            var refreshToken = new RefreshToken { UserId = 1, Token = "validToken", Expires = DateTime.UtcNow.AddDays(7) };
            var refreshTokens = new List<RefreshToken> { refreshToken }.AsQueryable();

            var mockSet = refreshTokens.BuildMockDbSet();
            _mockDbContext.Setup(db => db.RefreshToken).Returns(mockSet.Object);

            // Act
            var exception = await Record.ExceptionAsync(() => _tokenService.RevokeRefreshToken(nonExistentToken));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<SecurityTokenException>(exception);
        }

    }
}
