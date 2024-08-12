using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SampleAPI.Domain.Entities;
using SampleAPI.Domain.Repository;

namespace SampleAPI.Application.Query;

public class GetOrdersByDaysQueryHandler : IRequestHandler<GetOrdersByDaysQuery,IEnumerable<Order>>
{
    
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderDetailRepository _orderDetailRepository;

    public GetOrdersByDaysQueryHandler(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository)
    {
        _orderRepository = orderRepository;
        _orderDetailRepository = orderDetailRepository;
    }

    /// <summary>
    /// handle get order by days request, fetch order and order details
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Order>> Handle(GetOrdersByDaysQuery request, CancellationToken cancellationToken)
    {
        var date = GetDate(request.Days);
        var orders =  await _orderRepository.GetRecentOrders();
        var orderList = orders.ToList().Where(o=> o.OrderDate >= date).ToList();
        if (!orderList.Any()) return orderList;
        var orderIds = orderList.ToList().Select(o => o.OrderId);
        var orderDetails = await _orderDetailRepository.GetOrderDetails(orderIds);
        foreach (var order in orderList)
        {
            order.Products = orderDetails[order.OrderId];
        }
        return orderList;
    }
    
    
    /// <summary>
    /// calculate the date excluding weekends based on given no of days
    /// </summary>
    /// <param name="days"></param>
    /// <returns></returns>
    private static DateTime GetDate(int days)
    {
        var start = DateTime.Today;
        var date = Enumerable.Range(1,days*2)
            .Select(offset => start.AddDays(offset * -1))
            .Where(date => date.DayOfWeek != DayOfWeek.Saturday 
                           && date.DayOfWeek != DayOfWeek.Sunday).Take(days).Reverse().Take(1).FirstOrDefault();
        return date;
    }
}