using AutoMapper;
using Moq;
using SampleAPI.Application.Command;
using SampleAPI.Domain.Entities;
using SampleAPI.Domain.Repository;

namespace SampleAPI.Tests.Application.Command;

public class CreateOrderRequestCommandHandlerTest 
{
    private readonly Mock<IOrderRepository> _orderRepository;
    private readonly Mock<IOrderDetailRepository> _orderDetailRepository;
    private readonly CreateOrderRequestCommandHandler _handler;

    public CreateOrderRequestCommandHandlerTest()
    {
        _orderRepository = new Mock<IOrderRepository>();
        _orderDetailRepository = new Mock<IOrderDetailRepository>();
        var mappingConfig = new MapperConfiguration(mc => { mc.AddMaps("SampleApi"); });
        var mapper = mappingConfig.CreateMapper();
        _handler = new CreateOrderRequestCommandHandler(_orderRepository.Object, mapper, _orderDetailRepository.Object);

    }

    [Fact]
    public async Task ShouldCallRepositoryToAddNewOrderIfRequestIsValid()
    {
        var request = new CreateOrderRequestCommand()
        {
            OrderDate = new DateTime(2024, 5, 1),
            Products = new List<ProductRequestCommand>()
            {
                new ProductRequestCommand() {ProductId = 1, Quantity = 1},
                new ProductRequestCommand() {ProductId = 2, Quantity = 2}
            },
            CustomerId = "cus1",
            Name = "abc",
            Description = "sdsdd"
        };
        
        await _handler.Handle(request, CancellationToken.None);
        _orderRepository.Verify(x =>
            x.AddNewOrder(It.IsAny<Order>()), Times.Once);
        _orderDetailRepository.Verify(x =>
            x.AddOrderDetails(It.IsAny<int>(),It.IsAny<IEnumerable<Product>>()), Times.Once);
    }
}