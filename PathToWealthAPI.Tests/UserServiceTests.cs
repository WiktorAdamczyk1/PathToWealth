using Moq;
using Microsoft.EntityFrameworkCore;
using PathToWealthAPI.Data;
using PathToWealthAPI.Services;
using Microsoft.AspNetCore.Identity;
using MockQueryable.Moq;
using static PathToWealthAPI.Data.Models;

namespace PathToWealthAPI.Tests
{
    public class UserServiceTests
    {
        private readonly UserService _userService;
        private readonly Mock<IApplicationDbContext> _mockDbContext;
        private readonly Mock<DbSet<User>> _mockDbSet;
        private readonly Mock<IPasswordHasher<User>> _mockPasswordHasher;

        public UserServiceTests()
        {
            _mockDbContext = new Mock<IApplicationDbContext>();
            _mockPasswordHasher = new Mock<IPasswordHasher<User>>();
            _userService = new UserService(_mockDbContext.Object, _mockPasswordHasher.Object);
        }

        [Fact]
        public async Task GetUser_ReturnsUser_WhenUsernameOrEmailExists()
        {
            // Arrange
            var users = new List<User>
            {
                new User { UserId = 1, Username = "testuser1", Email = "test1@email.com" },
                new User { UserId = 2, Username = "testuser2", Email = "test2@email.com" }
            }.AsQueryable().BuildMockDbSet();

            _mockDbContext.Setup(db => db.User).Returns(users.Object);

            // Act
            var resultByUsername = await _userService.GetUser("testuser1");
            var resultByEmail = await _userService.GetUser("test2@email.com");

            // Assert
            Assert.NotNull(resultByUsername);
            Assert.Equal("testuser1", resultByUsername.Username);
            Assert.NotNull(resultByEmail);
            Assert.Equal("test2@email.com", resultByEmail.Email);
        }

        [Fact]
        public void VerifyPassword_ReturnsTrue_WhenPasswordIsCorrect()
        {
            // Arrange
            var user = new User { UserId = 1, Username = "testuser", PasswordHash = "hashedpassword" };
            _mockPasswordHasher.Setup(m => m.VerifyHashedPassword(user, user.PasswordHash, "password")).Returns(PasswordVerificationResult.Success);

            // Act
            var result = _userService.VerifyPassword(user, "password", _mockPasswordHasher.Object);

            // Assert
            Assert.True(result);
        }
    }
}
