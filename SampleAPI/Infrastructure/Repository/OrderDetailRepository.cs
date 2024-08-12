using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SampleAPI.Domain.Entities;
using SampleAPI.Domain.Repository;
using SampleAPI.Infrastructure.DAO;

namespace SampleAPI.Infrastructure.Repository;

public class OrderDetailRepository : IOrderDetailRepository
{
    private readonly IMapper _mapper;
    private readonly InMemoryDbContext _dbContext;

    public OrderDetailRepository(IMapper mapper, InMemoryDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }
    
    /// <summary>
    /// get order details by list of order ids
    /// </summary>
    /// <param name="orderIds"></param>
    /// <returns></returns>
    public async  Task<Dictionary<int, List<Product>>> GetOrderDetails(IEnumerable<int> orderIds)
    {
        var orderDetails = await _dbContext.OrderDetails.ToListAsync();
        var details = orderDetails.Where(o => orderIds.Contains(o.OrderId))
            .GroupBy(o=> o.OrderId)
            .ToDictionary(o=>o.Key,o=> _mapper.Map<List<OrderDetailsDao>, List<Product>>(o.ToList()));
        return  details;
    }
    
    /// <summary>
    /// add order details when a new order is added by user
    /// </summary>
    /// <param name="orderId"></param>
    /// <param name="orderDetails"></param>
    public async Task AddOrderDetails(int orderId, IEnumerable<Product> orderDetails)
    {
        foreach (var item in orderDetails)
        {
            var orderDetailDao = _mapper.Map<OrderDetailsDao>(item);
            orderDetailDao.OrderId = orderId;
            _dbContext.OrderDetails.Add(orderDetailDao);
        }
        await _dbContext.SaveChangesAsync();
    }
}