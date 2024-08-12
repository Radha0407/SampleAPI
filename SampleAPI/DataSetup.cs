using System;
using System.Collections.Generic;
using System.Linq;
using SampleAPI.Domain.Entities;
using SampleAPI.Infrastructure.DAO;

namespace SampleAPI;

public static class DataSetup
{
    public static void Initialize(InMemoryDbContext dbContext)
    {
        SeedOrderData(dbContext);
        SeedOrderDetailsData(dbContext);
        dbContext.SaveChanges();
    }

    private static void SeedOrderData(InMemoryDbContext dbContext)
    {
        if (dbContext.Orders.Any()) return;
        var orderData = new List<OrderDao>
        {
            new OrderDao()
            {
                OrderId =1,
                CustomerId = "cus1",
                OrderDate = new DateTime(2024,8,1),
                Description = "order1",
                Name = "abc"
            },
            new OrderDao()
            {
                OrderId =2,
                CustomerId = "cus1",
                OrderDate = new DateTime(2024,8,5),
                Description = "order2",
                Name = "bcd"
            },
            new OrderDao()
            {
                OrderId =3,
                CustomerId = "cus2",
                OrderDate = new DateTime(2024,8,4),
                Description = "order3",
                Name = "efg"
            }
        };
        dbContext.AddRange(orderData);
    }

    private static void SeedOrderDetailsData(InMemoryDbContext dbContext)
    {
        if (dbContext.OrderDetails.Any()) return;
        var orderData = new List<OrderDetailsDao>
        {
            new OrderDetailsDao
            {
                Id = 1,
                OrderId = 1,
                ProductId = 1,
                Quantity = 2
            },
            new OrderDetailsDao
            {
                Id = 2,
                OrderId = 1,
                ProductId = 2,
                Quantity = 1
            },
            new OrderDetailsDao
            {
                Id = 3,
                OrderId = 2,
                ProductId = 3,
                Quantity = 1
            },
            new OrderDetailsDao
            {
                Id = 4,
                OrderId = 3,
                ProductId = 2,
                Quantity = 2
            }
        };
        dbContext.AddRange(orderData);
    }
}