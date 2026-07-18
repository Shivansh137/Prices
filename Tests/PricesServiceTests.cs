using Microsoft.EntityFrameworkCore;
using PricesService;
using PricesService.Data;
using PricesService.Services;
using Xunit;

namespace Tests;

public class PriceServiceTests
{
    [Fact]
    public async Task GetPrice_ReturnsCorrectPrice_ForKnownItem()
    {
        // 1. Arrange: Set up a unique, isolated in-memory database instance
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestCarPricesDb_" + Guid.NewGuid().ToString())
            .Options;

        // 2. Initialize the context and force it to run OnModelCreating to seed the sample cars
        using var mockDbContext = new AppDbContext(options);
        mockDbContext.Database.EnsureCreated();

        // 3. Inject the mock context into the service constructor
        var service = new PriceService(mockDbContext);
        var request = new PriceRequest { Id = "porsche-911" };

        // 4. Act: Pass a mock ServerCallContext (null is acceptable for this basic implementation)
        var response = await service.GetPrice(request, null!);

        // 5. Assert: Verify the seeded price matches perfectly
        Assert.Equal(114400.00, response.Price);
    }
}