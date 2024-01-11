using Moq;
using Microsoft.EntityFrameworkCore;
using PathToWealthAPI.Data;
using PathToWealthAPI.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using MockQueryable.Moq;
using Microsoft.AspNetCore.Mvc;
using static PathToWealthAPI.Data.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace PathToWealthAPI.Tests
{
    public class UserFinancialDataServiceTests
    {
        private readonly Mock<IApplicationDbContext> _mockDbContext;
        private readonly UserFinancialDataService _service;
        private readonly Mock<HttpContext> _mockHttpContext;
        private readonly Mock<ClaimsPrincipal> _mockUser;

        public UserFinancialDataServiceTests()
        {
            _mockDbContext = new Mock<IApplicationDbContext>();
            _service = new UserFinancialDataService(_mockDbContext.Object);
            _mockHttpContext = new Mock<HttpContext>();
            _mockUser = new Mock<ClaimsPrincipal>();
            _mockHttpContext.Setup(hc => hc.User).Returns(_mockUser.Object);
        }


        [Fact]
        public async Task GetUserFinancialData_ReturnsUnauthorized_WhenUserIsNotAuthenticated()
        {
            // Arrange
            _mockUser.Setup(u => u.FindFirst(It.IsAny<string>())).Returns((Claim)null);

            // Act
            var result = await _service.GetUserFinancialData(_mockHttpContext.Object);

            // Assert
            Assert.IsType<UnauthorizedHttpResult>(result); 
        }


        [Fact]
        public async Task GetUserFinancialData_ReturnsNotFound_WhenUserHasNoFinancialData()
        {
            // Arrange
            var userId = 1;
            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            _mockUser.Setup(u => u.FindFirst(It.IsAny<string>())).Returns(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));
            var userFinancialData = new List<UserFinancialData>().AsQueryable().BuildMockDbSet();
            _mockDbContext.Setup(db => db.UserFinancialData).Returns(userFinancialData.Object);

            // Act
            var result = await _service.GetUserFinancialData(_mockHttpContext.Object);

            // Assert
            Assert.IsType<NotFound>(result);
        }


        [Fact]
        public async Task GetUserFinancialData_ReturnsOk_WhenUserHasFinancialData()
        {
            // Arrange
            var userId = 1;
            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            _mockUser.Setup(u => u.FindFirst(It.IsAny<string>())).Returns(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));
            var userFinancialData = new List<UserFinancialData>
            {
                new UserFinancialData { UserId = userId, Age = 30 }
            }.AsQueryable().BuildMockDbSet();
            _mockDbContext.Setup(db => db.UserFinancialData).Returns(userFinancialData.Object);

            // Act
            var result = await _service.GetUserFinancialData(_mockHttpContext.Object);

            // Assert
            var okResult = Assert.IsType<Ok<List<UserFinancialData>>>(result);
            Assert.Single(okResult.Value);
        }


        [Fact]
        public async Task UpdateUserFinancialData_ReturnsUnauthorized_WhenUserIsNotAuthenticated()
        {
            // Arrange
            _mockUser.Setup(u => u.FindFirst(It.IsAny<string>())).Returns((Claim)null);

            // Act
            var result = await _service.UpdateUserFinancialData(_mockHttpContext.Object, new UserFinancialData());

            // Assert
            Assert.IsType<UnauthorizedHttpResult>(result);
        }


        [Fact]
        public async Task UpdateUserFinancialData_ReturnsNotFound_WhenUserHasNoFinancialData()
        {
            // Arrange
            var userId = 1;
            _mockUser.Setup(u => u.FindFirst(It.IsAny<string>())).Returns(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));

            var userFinancialData = new List<UserFinancialData>().AsQueryable();
            var mockSet = userFinancialData.BuildMockDbSet();
            _mockDbContext.Setup(db => db.UserFinancialData).Returns(mockSet.Object);

            // Act
            var result = await _service.UpdateUserFinancialData(_mockHttpContext.Object, new UserFinancialData());

            // Assert
            Assert.IsType<NotFound>(result);
        }


        [Fact]
        public async Task UpdateUserFinancialData_ReturnsNoContent_WhenDataIsUpdated()
        {
            // Arrange
            var userId = 1;
            _mockUser.Setup(u => u.FindFirst(It.IsAny<string>())).Returns(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));
            var existingData = new UserFinancialData { UserId = userId, Age = 30 };
            var userFinancialData = new List<UserFinancialData> { existingData }.AsQueryable();
            var mockSet = userFinancialData.BuildMockDbSet();
            _mockDbContext.Setup(db => db.UserFinancialData).Returns(mockSet.Object);

            var cancellationToken = new CancellationToken();

            // Act
            var result = await _service.UpdateUserFinancialData(_mockHttpContext.Object, new UserFinancialData { Age = 35 });

            // Assert
            Assert.IsType<NoContent>(result);
            _mockDbContext.Verify(db => db.SaveChangesAsync(cancellationToken), Times.Once);
            Assert.Equal(35, existingData.Age);
        }


        [Fact]
        public async Task InsertUserFinancialData_AddsData_WhenCalled()
        {
            // Arrange
            var userId = 1;
            var newData = new UserFinancialData { Age = 30 };
            var mockUserFinancialDataSet = new Mock<DbSet<UserFinancialData>>();
            _mockDbContext.Setup(db => db.UserFinancialData).Returns(mockUserFinancialDataSet.Object);

            // Act
            await _service.InsertUserFinancialData(userId, newData);

            // Assert
            mockUserFinancialDataSet.Verify(db => db.Add(It.Is<UserFinancialData>(ufd => ufd.UserId == userId && ufd.Age == 30)), Times.Once);
            _mockDbContext.Verify(db => db.SaveChangesAsync(default), Times.Once);
        }

    }
}
