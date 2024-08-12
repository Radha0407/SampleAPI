using System;
using System.ComponentModel.DataAnnotations;

namespace SampleAPI.Infrastructure.DAO;

public class OrderDao
{
    [Key]
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public string? CustomerId { get; set; }
    public bool IsInvoiced { get; set; }
    public bool IsDeleted { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }

}