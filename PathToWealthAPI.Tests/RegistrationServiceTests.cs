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
        private readonly List<UserFinancialData> _userFinancialData;

        public RegistrationServiceTests()
        {
            _mockDbContext = new Mock<IApplicationDbContext>();
            _mockPasswordHasher = new Mock<IPasswordHasher<User>>();
            _registrationService = new RegistrationService(_mockDbContext.Object);
            _userFinancialData = new List<UserFinancialData>();

            var users = new List<User>().AsQueryable();
            var mockUserSet = users.BuildMockDbSet();
            _mockDbContext.Setup(x => x.User).Returns(mockUserSet.Object);
            var mockFinancialDataSet = _userFinancialData.AsQueryable().BuildMockDbSet();
            _mockDbContext.Setup(x => x.UserFinancialData).Returns(mockFinancialDataSet.Object);
            _mockDbContext.Setup(x => x.UserFinancialData.Add(It.IsAny<UserFinancialData>()))
                .Callback<UserFinancialData>(_userFinancialData.Add);
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

        [Fact]
        public async Task RegisterUser_ShouldCreateDefaultFinancialData_WhenNoFinancialDataProvided()
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

            // Assert that a UserFinancialData object was created
            var financialData = _userFinancialData.FirstOrDefault(x => x.UserId == result.UserId);
            Assert.NotNull(financialData);

            // Assert that the UserFinancialData object has the expected default values
            Assert.Equal(10000, financialData.InitialInvestment);
            Assert.Equal(DateTime.UtcNow.Year, financialData.StartInvestmentYear);
            Assert.Equal(DateTime.UtcNow.Year + 30, financialData.StartWithdrawalYear);
            Assert.False(financialData.IsInvestmentMonthly);
            Assert.Equal(12000, financialData.YearlyOrMonthlySavings);
            Assert.Equal(7.90M, financialData.StockAnnualReturn);
            Assert.Equal(0.04M, financialData.StockCostRatio);
            Assert.Equal(3.30M, financialData.BondAnnualReturn);
            Assert.Equal(0.05M, financialData.BondCostRatio);
            Assert.Equal(100, financialData.StockToBondRatio);
            Assert.Equal(30, financialData.RetirementDuration);
        }

    }
}
