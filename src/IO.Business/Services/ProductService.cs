using System;
using System.Threading.Tasks;
using IO.Business.Interfaces;
using IO.Business.Models;
using IO.Business.Validations;

namespace IO.Business.Services
{
    public class ProductService : BaseService, IProductService
    {
        public async Task Add(Product product)
        {
            if (!ExecuteValidation(new ProductValidation(), product)) return;
        }

        public async Task Update(Product product)
        {
            if (!ExecuteValidation(new ProductValidation(), product)) return;
        }

        public Task Remove(Guid productId)
        {
            throw new NotImplementedException();
        }
    }
}
