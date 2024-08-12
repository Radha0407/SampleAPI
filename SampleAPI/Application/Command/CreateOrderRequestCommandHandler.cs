using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SampleAPI.Domain.Entities;
using SampleAPI.Domain.Repository;

namespace SampleAPI.Application.Command;

public class CreateOrderRequestCommandHandler:  IRequestHandler<CreateOrderRequestCommand, int>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderDetailRepository _orderDetailRepository;
    private readonly IMapper _mapper;

    public CreateOrderRequestCommandHandler(IOrderRepository orderRepository, IMapper mapper, IOrderDetailRepository orderDetailRepository)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _orderDetailRepository = orderDetailRepository;
    }

    /// <summary>
    /// add order and order details when user creates a new order
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns> orderId </returns>
    public async Task<int> Handle(CreateOrderRequestCommand request, CancellationToken cancellationToken)
    {
        var order = _mapper.Map<Order>(request);
        order.IsInvoiced = true;
        order.IsDeleted = false;
        //ToDo: validate with the list of product requested, if the product id is valid and quantity requested is in stock
        var orderId = await _orderRepository.AddNewOrder(order);
        if (order.Products != null) 
            await _orderDetailRepository.AddOrderDetails(orderId, order.Products);
        return orderId;
    }
    
}