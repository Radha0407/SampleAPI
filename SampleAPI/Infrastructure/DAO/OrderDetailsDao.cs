using System.ComponentModel.DataAnnotations;

namespace SampleAPI.Infrastructure.DAO;

public class OrderDetailsDao
{
    [Key]
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}