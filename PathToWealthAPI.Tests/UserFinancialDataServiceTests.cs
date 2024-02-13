using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using PathToWealthAPI.Data;
using PathToWealthAPI.Services;
using MockQueryable.Moq;
using static PathToWealthAPI.Data.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace PathToWealthAPI.Tests
{
    public class UserFinancialDataServiceTests
    {
        private readonly Mock<IApplicationDbContext> _mockDbContext;

        public UserFinancialDataServiceTests()
        {
            _mockDbContext = new Mock<IApplicationDbContext>();
        }

        [Fact]
        public async Task GetUserFinancialData_ShouldReturnUnauthorizedForMissingUserClaim()
        {
            // Arrange
            var mockHttpContext = new Mock<HttpContext>();
            var mockService = new Mock<IUserFinancialDataService>();
            var userFinancialData = new List<UserFinancialData>().AsQueryable().BuildMockDbSet();

            mockService.Setup(s => s.GetUserFinancialData(mockHttpContext.Object)).ReturnsAsync(Results.Unauthorized());

            // Act
            var result = await mockService.Object.GetUserFinancialData(mockHttpContext.Object);

            // Assert
            Assert.IsType<UnauthorizedHttpResult>(result);
        }

        [Fact]
        public async Task GetUserFinancialData_ShouldReturnNotFoundForMissingFinancialData()
        {
            // Arrange
            var mockHttpContext = new Mock<HttpContext>();
            var mockService = new Mock<IUserFinancialDataService>();
            var userFinancialData = new List<UserFinancialData>().AsQueryable().BuildMockDbSet();

            mockService.Setup(s => s.GetUserFinancialData(mockHttpContext.Object)).ReturnsAsync(Results.NotFound());

            // Act
            var result = await mockService.Object.GetUserFinancialData(mockHttpContext.Object);

            // Assert
            Assert.IsType<NotFound>(result);
        }

        [Fact]
        public async Task GetUserFinancialData_ShouldReturnOkForExistingFinancialData()
        {
            // Arrange
            var mockHttpContext = new Mock<HttpContext>();
            var mockService = new Mock<IUserFinancialDataService>();
            var userFinancialData = new UserFinancialData();

            mockService.Setup(s => s.GetUserFinancialData(mockHttpContext.Object)).ReturnsAsync(Results.Ok(userFinancialData));

            // Act
            var result = await mockService.Object.GetUserFinancialData(mockHttpContext.Object);

            // Assert
            Assert.IsType<Ok<UserFinancialData>>(result);
        }

        [Fact]
        public async Task UpdateUserFinancialData_ShouldReturnUnauthorizedForMissingUserClaim()
        {
            // Arrange
            var mockDb = new Mock<IApplicationDbContext>();
            var service = new UserFinancialDataService(mockDb.Object);
            var httpContext = new DefaultHttpContext();

            // Act
            var result = await service.UpdateUserFinancialData(httpContext, new UserFinancialData());

            // Assert
            Assert.IsType<UnauthorizedHttpResult>(result);
        }

        [Fact]
        public async Task UpdateUserFinancialData_ShouldReturnNotFoundForMissingFinancialData()
        {
            // Arrange
            var userId = 1;
            var mockDb = new Mock<IApplicationDbContext>();
            var data = new List<UserFinancialData>().AsQueryable().BuildMockDbSet();
            mockDb.Setup(db => db.UserFinancialData).Returns(data.Object);
            var service = new UserFinancialDataService(mockDb.Object);
            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) }));

            // Act
            var result = await service.UpdateUserFinancialData(httpContext, new UserFinancialData());

            // Assert
            Assert.IsType<NotFound>(result);
        }

        [Fact]
        public async Task UpdateUserFinancialData_ShouldUpdateDataAndReturnNoContent()
        {
            // Arrange
            var userId = 1;
            var mockDb = new Mock<IApplicationDbContext>();
            var data = new List<UserFinancialData> { new UserFinancialData { UserId = userId } }.AsQueryable().BuildMockDbSet();
            mockDb.Setup(db => db.UserFinancialData).Returns(data.Object);
            var service = new UserFinancialDataService(mockDb.Object);
            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) }));

            // Act
            var result = await service.UpdateUserFinancialData(httpContext, new UserFinancialData());

            // Assert
            Assert.IsType<NoContent>(result);
        }

        [Fact]
        public async Task InsertUserFinancialData_ShouldInsertDataCorrectly()
        {
            // Arrange
            var mockDb = new Mock<IApplicationDbContext>();
            var mockSet = new Mock<DbSet<UserFinancialData>>();
            mockDb.Setup(db => db.UserFinancialData).Returns(mockSet.Object);

            var service = new UserFinancialDataService(mockDb.Object);
            var userId = 1;
            var userFinancialData = new UserFinancialData();

            // Act
            await service.InsertUserFinancialData(userId, userFinancialData);

            // Assert
            mockSet.Verify(m => m.Add(It.Is<UserFinancialData>(u => u == userFinancialData)), Times.Once());
            mockDb.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task InsertUserFinancialData_ShouldSetUserIdBeforeInsertion()
        {
            // Arrange
            var mockDb = new Mock<IApplicationDbContext>();
            var mockSet = new Mock<DbSet<UserFinancialData>>();
            mockDb.Setup(db => db.UserFinancialData).Returns(mockSet.Object);

            var service = new UserFinancialDataService(mockDb.Object);
            var userId = 1;
            var userFinancialData = new UserFinancialData();

            // Act
            await service.InsertUserFinancialData(userId, userFinancialData);

            // Assert
            Assert.Equal(userId, userFinancialData.UserId);
            mockSet.Verify(m => m.Add(It.Is<UserFinancialData>(u => u.UserId == userId)), Times.Once());
            mockDb.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
