using AutoMapper;
using SampleAPI.Domain.Entities;
using SampleAPI.Infrastructure.Repository;

namespace SampleAPI.Tests.Repositories;

public class OrderDetailsRepositoryTests
{
    private readonly OrderDetailRepository _repository;
    private readonly InMemoryDbContext _dbContext;

    public OrderDetailsRepositoryTests()
    {
        _dbContext = MockApiDbContextFactory.GenerateMockContext( "mock_OrderDetailDbContext");
        var mappingConfig = new MapperConfiguration(mc => { mc.AddMaps("SampleApi"); });
        var mapper = mappingConfig.CreateMapper();
        _repository = new OrderDetailRepository(mapper,_dbContext);
    }
    
    [Fact]
    public async Task ShouldGetOrderDetailsByOrderIds()
    {
        const int orderId = 1;
        var orderDetails = await _repository.GetOrderDetails(new List<int>(){orderId});
        Assert.NotNull(orderDetails);
        Assert.NotEmpty(orderDetails);
        Assert.True(orderDetails.ContainsKey(orderId));
    }
    
    [Fact]
    public async Task ShouldAddNewOrderSuccessfully()
    {
        var request = new List<Product>()
        {
            new Product() {ProductId = 1, Quantity = 1},
            new Product() {ProductId = 2, Quantity = 1}
        };
        const int orderId = 6;
        await _repository.AddOrderDetails(orderId,request);
        var orderDetails = _dbContext.OrderDetails.Where(o => o.OrderId == orderId).ToList();

        foreach (var item in request)
        {
            var detail = orderDetails?.FirstOrDefault(o => o.ProductId == item.ProductId);
            Assert.NotNull(detail);
            Assert.Equal(item.ProductId,detail.ProductId);
            Assert.Equal(item.Quantity,detail.Quantity);
        }
    }
}