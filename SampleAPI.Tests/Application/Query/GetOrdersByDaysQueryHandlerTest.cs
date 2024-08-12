using Moq;
using SampleAPI.Application.Query;
using SampleAPI.Domain.Entities;
using SampleAPI.Domain.Repository;

namespace SampleAPI.Tests.Application.Query;

public class GetOrdersByDaysQueryHandlerTest
{
    private readonly Mock<IOrderRepository> _orderRepository;
    private readonly Mock<IOrderDetailRepository> _orderDetailRepository;
    private readonly GetOrdersByDaysQueryHandler _handler;
    
    public GetOrdersByDaysQueryHandlerTest()
    {
        _orderRepository = new Mock<IOrderRepository>();
        _orderDetailRepository = new Mock<IOrderDetailRepository>();
        _handler = new GetOrdersByDaysQueryHandler(_orderRepository.Object, _orderDetailRepository.Object);
    }
    
    [Fact]
    public async Task ShouldFetchOrderAndOrderDetailsToGetRecentOrders()
    {
        var request = new GetOrdersByDaysQuery()
        {
            Days = 2
        };
        _orderRepository.Setup(x => x.GetRecentOrders())
            .ReturnsAsync(new List<Order>()
                {
                    new Order(){ OrderId = 1, OrderDate = DateTime.Now,CustomerId = "cus",Description = "abc",Name = "abc"}
                });
        _orderDetailRepository.Setup(x => x.GetOrderDetails(It.IsAny<IEnumerable<int>>()))
            .ReturnsAsync(new Dictionary<int, List<Product>>()
            {
                {1,new List<Product>(){new Product(){ProductId = 1,Quantity = 2}}}
            });
        await _handler.Handle(request, CancellationToken.None);
        _orderRepository.Verify(x =>
            x.GetRecentOrders(), Times.Once);
        _orderDetailRepository.Verify(x =>
            x.GetOrderDetails(It.IsAny<IEnumerable<int>>()), Times.Once);
    }
    
    [Fact]
    public async Task ShouldNotFetchOrderDetailsIfNoOrdersFound()
    {
        var request = new GetOrdersByDaysQuery()
        {
            Days = 2
        };
        _orderRepository.Setup(x => x.GetRecentOrders())
            .ReturnsAsync(new List<Order>());
        _orderDetailRepository.Setup(x => x.GetOrderDetails(It.IsAny<IEnumerable<int>>()))
            .ReturnsAsync(new Dictionary<int, List<Product>>()
            {
                {1,new List<Product>(){new Product(){ProductId = 1,Quantity = 2}}}
            });
        await _handler.Handle(request, CancellationToken.None);
        _orderRepository.Verify(x =>
            x.GetRecentOrders(), Times.Once);
        _orderDetailRepository.Verify(x =>
            x.GetOrderDetails(It.IsAny<IEnumerable<int>>()), Times.Never);
    }
    
}