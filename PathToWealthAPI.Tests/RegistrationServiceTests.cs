using Microsoft.AspNetCore.Identity;
using MockQueryable.Moq;
using Moq;
using PathToWealthAPI.Data;
using PathToWealthAPI.Services;
using static PathToWealthAPI.Data.Models;

namespace PathToWealthAPI.Tests
{
    public class RegistrationServiceTests
    {
        private readonly Mock<IApplicationDbContext> _mockDbContext;
        private readonly Mock<IPasswordHasher<User>> _mockPasswordHasher;
        private readonly RegistrationService _registrationService;

        public RegistrationServiceTests()
        {
            _mockDbContext = new Mock<IApplicationDbContext>();
            _mockPasswordHasher = new Mock<IPasswordHasher<User>>();
            _registrationService = new RegistrationService(_mockDbContext.Object);

            // Create a list of users for mocking purposes
            var users = new List<User>().AsQueryable();
            var userFinancialData = new List<UserFinancialData>().AsQueryable();

            // Use BuildMockDbSet provided by MockQueryable.Moq
            var mockUserSet = users.BuildMockDbSet();
            var mockFinancialDataSet = userFinancialData.BuildMockDbSet();

            // Setup the mock context to return the mock set
            _mockDbContext.Setup(x => x.User).Returns(mockUserSet.Object);
            _mockDbContext.Setup(x => x.UserFinancialData).Returns(mockFinancialDataSet.Object);
        }

        [Fact]
        public async Task RegisterUser_ShouldReturnUser_WhenRegistrationIsSuccessful()
        {
            // Arrange
            var registration = new UserRegistration { Username = "test", Email = "test@test.com", Password = "password" };
            var hashedPassword = "hashedPassword";
            _mockPasswordHasher.Setup(x => x.HashPassword(null, registration.Password)).Returns(hashedPassword);

            // Act
            var result = await _registrationService.RegisterUser(registration, _mockPasswordHasher.Object);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(registration.Username, result.Username);
            Assert.Equal(registration.Email, result.Email);
            Assert.Equal(hashedPassword, result.PasswordHash);
        }

        [Fact]
        public async Task RegisterUser_ShouldThrowException_WhenUserAlreadyExists()
        {
            // Arrange
            var registration = new UserRegistration { Username = "test", Email = "test@test.com", Password = "password" };
            var existingUser = new User { Username = "test", Email = "test@test.com" };

            // Update the mock DbSet to contain the existing user
            var users = new List<User> { existingUser }.AsQueryable();
            var mockSet = users.BuildMockDbSet();
            _mockDbContext.Setup(x => x.User).Returns(mockSet.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _registrationService.RegisterUser(registration, _mockPasswordHasher.Object));
        }
    }
}
