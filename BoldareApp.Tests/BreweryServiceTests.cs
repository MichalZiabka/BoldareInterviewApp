using BoldareApp.Infrastructure.DbCache;
using BoldareApp.Queries;
using BoldareApp.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace BoldareApp.Tests
{
    public class BreweryServiceTests
    {
        //Example test

        [Fact]
        public async Task GetBreweriesAsync_ShouldReturnData()
        {
            var dbOptions = new DbContextOptionsBuilder<DbCacheContext>()
           .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
           .Options;

            using var dbContext = new DbCacheContext(dbOptions);

            // Arrange
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var loggerMock = new Mock<ILogger<BreweryService>>();
            var httpClient = new HttpClient(new FakeHttpMessageHandler());
            var service = new BreweryService(httpClient, memoryCache, loggerMock.Object, dbContext);

            var query = new FilterQuery { Name = "Test", City = "Katowice" };

            // Act
            var call = await service.GetBreweriesAsync(query);
            var secondCall = await service.GetBreweriesAsync(query);

            // Assert
            Assert.NotNull(call);
            call.Should().HaveCount(1);
        }
    }
}