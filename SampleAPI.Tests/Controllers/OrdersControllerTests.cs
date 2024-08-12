using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SampleAPI.Application.Command;
using SampleAPI.Application.Query;
using SampleAPI.Controllers;
using SampleAPI.Domain.Entities;

namespace SampleAPI.Tests.Controllers
{
    public class OrdersControllerTests
    {
        private readonly OrdersController _controller;
        private readonly Mock<IValidator<CreateOrderRequestCommand>> _validator;
        private readonly Mock<IMediator> _mockMediator;
        
        public OrdersControllerTests()
        {
            _validator = new Mock<IValidator<CreateOrderRequestCommand>>();
            _mockMediator = new Mock<IMediator>();
            Mock<ILogger<OrdersController>> logger = new();
            _controller = new OrdersController(_validator.Object,logger.Object,_mockMediator.Object);
        }

        [Fact]
        public async Task ShouldSendCommandToHandlerForAddingNewOrderIfRequestIsValid()
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

            _validator.Setup(x => x.ValidateAsync(It.IsAny<CreateOrderRequestCommand>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult()).Verifiable();
            _mockMediator.Setup(x => x.Send(It.IsAny<CreateOrderRequestCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1).Verifiable();
            
            var result = await _controller.CreateOrder(request);
            _mockMediator.Verify(x=> x.Send(It.IsAny<CreateOrderRequestCommand>(),new CancellationToken()),Times.Once);
            ((ObjectResult)result).StatusCode.Should().Be(200);
        }
        
        [Fact]
        public async Task ShouldReturnBadRequestIfCreateOrderRequestIsInvalid()
        {
            var request = new CreateOrderRequestCommand()
            {
                OrderDate = new DateTime(),
                Products = new List<ProductRequestCommand>()
                {
                    new ProductRequestCommand() {ProductId = 1, Quantity = 1},
                    new ProductRequestCommand() {ProductId = 2, Quantity = 2}
                },
                CustomerId = "cus1",
                Name = "def",
                Description = "sdssvs"
            };

            _validator.Setup(x => x.ValidateAsync(It.IsAny<CreateOrderRequestCommand>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult(
                new List<ValidationFailure>
                {
                    new("Error", "Error Message")
                }
            ));
            var result = async () =>  await _controller.CreateOrder(request);
            await result.Should().ThrowAsync<BadHttpRequestException>()
                .WithMessage("Request is not valid - Error Message");
        }

        [Fact]
        public async Task ShouldSendQueryToHandlerToGetAllRecentOrders()
        {
            _mockMediator.Setup(x => x.Send(It.IsAny<GetRecentOrdersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Order>()).Verifiable();
            await _controller.GetOrders();
            _mockMediator.Verify(x=> x.Send(It.IsAny<GetRecentOrdersQuery>(),new CancellationToken()),Times.Once);
        }
        
        [Fact]
        public async Task ShouldSendQueryToHandlerToGetOrdersByDate()
        {
            _mockMediator.Setup(x => x.Send(It.IsAny<GetRecentOrdersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Order>()).Verifiable();
            await _controller.GetOrders();
            _mockMediator.Verify(x=> x.Send(It.IsAny<GetRecentOrdersQuery>(),new CancellationToken()),Times.Once);
        }
        
    }
}
