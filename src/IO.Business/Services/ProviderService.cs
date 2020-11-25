using System;
using System.Threading.Tasks;
using IO.Business.Interfaces;
using IO.Business.Models;
using IO.Business.Validations;

namespace IO.Business.Services
{
    public class ProviderService : BaseService, IProviderService
    {
        public async Task Add(Provider provider)
        {
            //Validar o estado da entidade
            if (!ExecuteValidation(new ProviderValidation(), provider)
                && !ExecuteValidation(new AddressValidation(), provider.Address)) return;

            //se não existe fornecedor com o mesmo documento
            return;
        }

        public async Task Update(Provider provider)
        {
            if (!ExecuteValidation(new ProviderValidation(), provider)) return;
        }

        public async Task UpdateAddress(Address address)
        {
            if (!ExecuteValidation(new  AddressValidation(), address)) return;
        }

        public Task Remove(Guid providerId)
        {
            throw new NotImplementedException();

        }
    }
}
