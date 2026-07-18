using Grpc.Core;
using PricesService;

namespace PricesService.Services;

public class PriceService : PriceCalculator.PriceCalculatorBase
{
    public override Task<PriceResponse> GetPrice(PriceRequest request, ServerCallContext context)
    {
        // Production applications pull from DB/Cache. For this setup, we mock it.
        double targetPrice = request.Id == "item-100" ? 99.99 : 19.50;

        return Task.FromResult(new PriceResponse
        {
            Id = request.Id,
            Price = targetPrice
        });
    }
}