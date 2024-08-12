using System.Collections.Generic;
using SampleAPI.Infrastructure.DAO;

namespace SampleAPI.Infrastructure.Repository;

public class ProductRepository
{
    public IEnumerable<ProductDao> GetProductsById(List<string> productId)
    {
        return new List<ProductDao>();
    }
}