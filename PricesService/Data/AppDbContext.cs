using Microsoft.EntityFrameworkCore;

namespace PricesService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<CarPrice> CarPrices { get; set; }

    protected override void OnModelCreating(ModelCreatingBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed some initial Car data automatically when the database is created
        modelBuilder.Entity<CarPrice>().HasData(
            new CarPrice { Id = "tesla-model3", CarName = "Tesla Model 3", Price = 38990.00 },
            new CarPrice { Id = "porsche-911", CarName = "Porsche 911 Carrera", Price = 114400.00 },
            new CarPrice { Id = "mustang-gt", CarName = "Ford Mustang GT", Price = 42495.00 }
        );
    }
}