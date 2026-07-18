using Microsoft.EntityFrameworkCore;
using PricesService.Data;
using PricesService.Services;

var builder = WebApplication.CreateBuilder(args);

// Grab the connection string provided by Kubernetes environment variables
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "server=mysql-service;port=3306;database=CarPricesDb;user=root;password=SuperSecretPassword123!";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddGrpc();

var app = builder.Build();

// AUTOMATION GATE: Ensure the database is created and seeded automatically on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated(); 
}

app.MapGrpcService<PriceService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

app.Run();