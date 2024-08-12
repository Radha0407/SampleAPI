using System;
using System.Collections.Generic;

namespace SampleAPI.Domain.Request;

public class CreateOrderRequest
{
    public DateTime OrderDate { get; set; }
    public List<ProductRequest>? Products { get; set; }
    public string? CustomerId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}

public class ProductRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
