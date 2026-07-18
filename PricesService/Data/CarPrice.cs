using System.ComponentModel.DataAnnotations;

namespace PricesService.Data;

public class CarPrice
{
    [Key]
    public string Id { get; set; } = null!;
    public string CarName { get; set; } = null!;
    public double Price { get; set; }
}