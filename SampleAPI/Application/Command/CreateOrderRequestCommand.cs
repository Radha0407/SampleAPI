using System;
using System.Collections.Generic;
using MediatR;

namespace SampleAPI.Application.Command;

public class CreateOrderRequestCommand : IRequest<int>
{
    public DateTime OrderDate { get; set; }
    public List<ProductRequestCommand>? Products { get; set; }
    public string? CustomerId { get; set; }
    public string? Description { get; set; }
    public string? Name { get; set; }
}

public class ProductRequestCommand
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
