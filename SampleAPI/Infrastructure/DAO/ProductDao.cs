using System.ComponentModel.DataAnnotations;

namespace SampleAPI.Infrastructure.DAO;

public class ProductDao
{
    [Key]
    public int ProductId { get; set; }
    public string? Name { get; set; }
    public double Price { get; set; }
    public int Stock { get; set; }
}