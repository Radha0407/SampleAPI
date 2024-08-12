using System;
using System.Collections.Generic;

namespace SampleAPI.Domain.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<Product>?  Products { get; set; }
        public string? CustomerId { get; set; }
        public bool IsInvoiced { get; set; }
        public bool IsDeleted { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
