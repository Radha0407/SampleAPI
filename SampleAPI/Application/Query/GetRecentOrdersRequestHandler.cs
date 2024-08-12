using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SampleAPI.Domain.Entities;
using SampleAPI.Domain.Repository;

namespace SampleAPI.Application.Query;

public class GetRecentOrdersQueryHandler : IRequestHandler<GetRecentOrdersQuery,IEnumerable<Order>>
{
    
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderDetailRepository _orderDetailRepository;

    public GetRecentOrdersQueryHandler(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository)
    {
        _orderRepository = orderRepository;
        _orderDetailRepository = orderDetailRepository;
    }

    /// <summary>
    /// handle get all recent orders request, fetch order and order details
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Order>> Handle(GetRecentOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders =  await _orderRepository.GetRecentOrders();
        var orderList = orders.ToList();
        if (!orderList.Any()) return orderList;
        var orderIds = orderList.ToList().Select(o => o.OrderId);
        var orderDetails = await _orderDetailRepository.GetOrderDetails(orderIds);
        foreach (var order in orderList)
        {
            order.Products = orderDetails[order.OrderId];
        }
        return orderList;
    }
}