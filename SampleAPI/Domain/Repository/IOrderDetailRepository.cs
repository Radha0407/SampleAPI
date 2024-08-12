using System.Collections.Generic;
using System.Threading.Tasks;
using SampleAPI.Domain.Entities;

namespace SampleAPI.Domain.Repository;

public interface IOrderDetailRepository
{
    public Task<Dictionary<int, List<Product>>> GetOrderDetails(IEnumerable<int> orderIds);
    public Task AddOrderDetails(int orderId, IEnumerable<Product> orderDetails);
}