using System;
using System.Threading.Tasks;
using IO.Business.Models;

namespace IO.Business.Interfaces
{
    public interface IProviderRepository : IRepository<Provider>
    {
        Task<Provider> SearchProviderAddress(Guid providerId);

        Task<Provider> SearchProviderProductsAddress(Guid providerId);
    }
}
