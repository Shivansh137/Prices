using PricesService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add gRPC framework services to the internal IoC container
builder.Services.AddGrpc();

var app = builder.Build();

// Route incoming gRPC traffic directly to our core business service
app.MapGrpcService<PriceService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

app.Run();