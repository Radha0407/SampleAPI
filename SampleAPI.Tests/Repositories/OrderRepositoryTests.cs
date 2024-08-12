using AutoMapper;
using SampleAPI.Domain.Entities;
using SampleAPI.Infrastructure.Repository;

namespace SampleAPI.Tests.Repositories
{
    public class OrderRepositoryTests
    {
        private readonly InMemoryDbContext _dbContext;
        private readonly OrderRepository _repository;

        public OrderRepositoryTests()
        {
            _dbContext =  MockApiDbContextFactory.GenerateMockContext("mock_OrderDbContext");
            var mappingConfig = new MapperConfiguration(mc => { mc.AddMaps("SampleApi"); });
            var mapper = mappingConfig.CreateMapper();
            _repository = new OrderRepository(mapper,_dbContext);
        }

        [Fact]
        public async Task ShouldAddNewOrderSuccessfully()
        {
            var request = new Order()
            {

                OrderDate = new DateTime(2024, 5, 1),
                Products = new List<Product>()
                {
                    new Product() {ProductId = 1, Quantity = 1},
                    new Product() {ProductId = 2, Quantity = 1}
                },
                CustomerId = "cus1",
                Description = "order sample",
                Name = "abc"
            };
             var orderId = await _repository.AddNewOrder(request);

             var order = _dbContext.Orders.FirstOrDefault(o => o.OrderId == orderId);
             Assert.NotNull(order);
             Assert.Equal(request.OrderDate,order.OrderDate);
             Assert.Equal(request.CustomerId,order.CustomerId);
             Assert.Equal(request.Description,order.Description);
             Assert.Equal(request.Name,order.Name);
             Assert.True(order.IsInvoiced);
             Assert.False(order.IsDeleted);
        }
        
        [Fact]
        public async Task ShouldGetRecentOrderListInDescendingOrder()
        {

            var recentOrders = await _repository.GetRecentOrders();
            Assert.NotNull(recentOrders);
            Assert.NotEmpty(recentOrders);

        }
    }
}