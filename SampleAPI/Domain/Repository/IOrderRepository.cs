using System.Collections.Generic;
using System.Threading.Tasks;
using SampleAPI.Domain.Entities;

namespace SampleAPI.Domain.Repository
{
    public interface IOrderRepository
    {
         public Task<IEnumerable<Order>>  GetRecentOrders();
         public Task<int> AddNewOrder(Order order);
         // public Task<IEnumerable<Order>> GetOrdersByDays(int days);
    }
}
