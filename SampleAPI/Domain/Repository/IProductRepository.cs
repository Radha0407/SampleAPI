using System;
using System.Collections.Generic;
using SampleAPI.Infrastructure.DAO;

namespace SampleAPI.Domain.Repository;

public interface IProductRepository
{
    public IEnumerable<ProductDao> GetProductsById(List<String> productId);
}