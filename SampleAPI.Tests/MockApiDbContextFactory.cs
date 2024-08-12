using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SampleAPI.Domain.Entities;
using SampleAPI.Infrastructure.DAO;

namespace SampleAPI.Tests
{
    /// <summary>
    /// Mock database provided for your convenience.
    /// </summary>
    internal static class MockApiDbContextFactory
    {
        public static InMemoryDbContext GenerateMockContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<InMemoryDbContext>()
                .UseInMemoryDatabase(dbName)
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            var context = new InMemoryDbContext(options);
            Initialize(context);
            return context;
        }

        private static void Initialize(InMemoryDbContext dbContext)
        {
            SeedOrder(dbContext);
            SeedOrderDetails(dbContext);
            dbContext.SaveChanges();
        }

        private static void SeedOrder(InMemoryDbContext dbContext)
        {
            if (dbContext.Orders.Any()) return;
            var orderData = new List<OrderDao>
            {
                new OrderDao()
                {
                    OrderId = 1,
                    CustomerId = "cus1",
                    OrderDate = DateTime.Now,
                    Description = "order2",
                    Name = "bcd"
                }
            };
            dbContext.AddRange(orderData);
        }
        
        private static void SeedOrderDetails(InMemoryDbContext dbContext)
        {
            if (dbContext.OrderDetails.Any()) return;
            var orderDetails = new List<OrderDetailsDao>
            {
                new OrderDetailsDao
                {
                    OrderId = 1,
                    ProductId = 1,
                    Quantity = 2
                },
                new OrderDetailsDao
                {
                    OrderId = 1,
                    ProductId = 2,
                    Quantity = 1
                }
            };
            dbContext.AddRange(orderDetails);
        }
    }
}
