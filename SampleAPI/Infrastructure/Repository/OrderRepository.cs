using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SampleAPI.Domain.Entities;
using SampleAPI.Domain.Repository;
using SampleAPI.Infrastructure.DAO;

namespace SampleAPI.Infrastructure.Repository
{
    public class OrderRepository : IOrderRepository
    {

        private readonly IMapper _mapper;
        private readonly InMemoryDbContext _dbContext;

        public OrderRepository(IMapper mapper, InMemoryDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        
        /// <summary>
        /// get recent orders and order details in descending order
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Order>> GetRecentOrders()
        {
            var orders =  await _dbContext.Orders.OrderByDescending(o => o.OrderDate).ToListAsync();
            var result = _mapper.Map<IEnumerable<OrderDao>, IEnumerable<Order>>(orders);
            return result ?? new List<Order>();

        }

        /// <summary>
        /// add new order and order details as one to many mapping
        /// </summary>
        /// <param name="order"></param>
        /// <returns> orderId </returns>
        public async Task<int> AddNewOrder(Order order)
        {
            var orderDao = _mapper.Map<OrderDao>(order);
            orderDao.IsInvoiced = true;
            _dbContext.Orders.Add(orderDao);
            await _dbContext.SaveChangesAsync();
            return orderDao.OrderId;
        }
    }
}
