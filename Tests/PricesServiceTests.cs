using PricesService;
using PricesService.Services;
using Xunit;

namespace Tests;

public class PriceServiceTests
{
    [Fact]
    public async Task GetPrice_ReturnsCorrectPrice_ForKnownItem()
    {
        // Arrange
        var service = new PriceService();
        var request = new PriceRequest { Id = "item-100" };

        // Act (We pass null for ServerCallContext since our simple mock doesn't use it)
        var response = await service.GetPrice(request, null);

        // Assert
        Assert.Equal(99.99, response.Price);
    }
}