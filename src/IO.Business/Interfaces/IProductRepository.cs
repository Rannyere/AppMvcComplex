using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IO.Business.Models;

namespace IO.Business.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByProvider(Guid providerId);

        Task<IEnumerable<Product>> GetProductsProvider();

        Task<Product> GetProductProvider(Guid id);
    }
}
