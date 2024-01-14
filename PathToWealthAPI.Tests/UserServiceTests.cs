using Moq;
using Microsoft.EntityFrameworkCore;
using PathToWealthAPI.Data;
using PathToWealthAPI.Services;
using Microsoft.AspNetCore.Identity;
using MockQueryable.Moq;
using static PathToWealthAPI.Data.Models;
using Xunit;

namespace PathToWealthAPI.Tests
{
    public class UserServiceTests
    {
        private UserService _userService;
        private Mock<DbSet<User>> _mockSet;
        private Mock<IApplicationDbContext> _mockDbContext;
        private readonly Mock<IPasswordHasher<User>> _mockPasswordHasher;

        public UserServiceTests()
        {
            var users = new List<User>
            {
                new User { UserId = 1, Username = "User1", Email = "user1@email.com", PasswordHash = "Password1" },
                new User { UserId = 2, Username = "User2", Email = "user2@email.com", PasswordHash = "Password2" },
                // Add more users as needed
            }.AsQueryable();

            _mockSet = new Mock<DbSet<User>>();
            _mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            _mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            _mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            _mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            // Setup FindAsync to return the user with ID 1
            _mockSet.Setup(m => m.FindAsync(1)).Returns(new ValueTask<User>(users.First(u => u.UserId == 1)));

            _mockDbContext = new Mock<IApplicationDbContext>();
            _mockDbContext.Setup(c => c.User).Returns(_mockSet.Object);
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
        public async Task GetUser_ReturnsNull_WhenUsernameOrEmailDoesNotExist()
        {
            // Arrange
            var users = new List<User>
            {
                new User { UserId = 1, Username = "testuser1", Email = "test1@email.com" },
                new User { UserId = 2, Username = "testuser2", Email = "test2@email.com" }
            }.AsQueryable().BuildMockDbSet();

            _mockDbContext.Setup(db => db.User).Returns(users.Object);

            // Act
            var result = await _userService.GetUser("nonexistentuser");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetUser_ThrowsException_WhenInputIsNull()
        {
            // Arrange
            string nullInput = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _userService.GetUser(nullInput));
        }

        [Fact]
        public async Task VerifyPassword_ReturnsTrue_WhenPasswordIsCorrect()
        {
            // Arrange
            var user = await _mockSet.Object.FindAsync(1);
            _mockPasswordHasher.Setup(m => m.VerifyHashedPassword(user, user.PasswordHash, "password")).Returns(PasswordVerificationResult.Success);

            // Act
            var result = _userService.VerifyPassword(user, "password", _mockPasswordHasher.Object);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task VerifyPassword_ReturnsFalse_WhenPasswordIsIncorrect()
        {
            // Arrange
            var user = await _mockSet.Object.FindAsync(1);
            _mockPasswordHasher.Setup(m => m.VerifyHashedPassword(user, user.PasswordHash, "wrongpassword")).Returns(PasswordVerificationResult.Failed);

            // Act
            var result = _userService.VerifyPassword(user, "wrongpassword", _mockPasswordHasher.Object);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void VerifyPassword_ThrowsException_WhenUserIsNull()
        {
            // Arrange
            User nullUser = null;

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _userService.VerifyPassword(nullUser, "password", _mockPasswordHasher.Object));
            Assert.Equal("user", exception.ParamName);
        }

        [Fact]
        public void VerifyPassword_ThrowsException_WhenPasswordIsNull()
        {
            // Arrange
            string nullPassword = null;

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var user = await _mockSet.Object.FindAsync(1);
                _userService.VerifyPassword(user, nullPassword, _mockPasswordHasher.Object);
            });

            Assert.Equal("password", exception.Result.ParamName);
        }

        [Fact]
        public async Task UpdateUser_UpdatesUserDetails()
        {
            var passwordHasher = new Mock<IPasswordHasher<User>>();
            passwordHasher.Setup(p => p.HashPassword(It.IsAny<User>(), It.IsAny<string>())).Returns("NewPasswordHash");

            await _userService.UpdatePassword(1, "NewPassword", passwordHasher.Object);

            _mockSet.Verify(m => m.Update(It.Is<User>(u => u.PasswordHash == "NewPasswordHash")), Times.Once);
            _mockDbContext.Verify(m => m.SaveChangesAsync(default(CancellationToken)), Times.Once);
        }

        [Fact]
        public async Task UpdateUser_ThrowsException_WhenUserDoesNotExist()
        {
            var passwordHasher = new Mock<IPasswordHasher<User>>();
            passwordHasher.Setup(p => p.HashPassword(It.IsAny<User>(), It.IsAny<string>())).Returns("NewPasswordHash");

            await Assert.ThrowsAsync<Exception>(() => _userService.UpdatePassword(3, "NewPassword", passwordHasher.Object));
        }

        [Fact]
        public async Task DeleteUser_DeletesUserFromDatabase()
        {
            await _userService.DeleteUser(1);

            _mockSet.Verify(m => m.Remove(It.Is<User>(u => u.UserId == 1)), Times.Once);
            _mockDbContext.Verify(m => m.SaveChangesAsync(default(CancellationToken)), Times.Once);
        }

        [Fact]
        public async Task DeleteUser_ThrowsException_WhenUserDoesNotExist()
        {
            await Assert.ThrowsAsync<Exception>(() => _userService.DeleteUser(3));
        }
    }
}
