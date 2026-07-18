using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using PricesService.Data;

namespace PricesService.Services;

public class PriceService : PriceCalculator.PriceCalculatorBase
{
    private readonly AppDbContext _dbContext;

    public PriceService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override async Task<PriceResponse> GetPrice(PriceRequest request, ServerCallContext context)
    {
        // Query MySQL for the exact car ID
        var car = await _dbContext.CarPrices.FirstOrDefaultAsync(c => c.Id == request.Id);

        if (car == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Car with ID '{request.Id}' not found."));
        }

        return new PriceResponse
        {
            Id = car.Id,
            Price = car.Price
        };
    }
}