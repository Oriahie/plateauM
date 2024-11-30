using PlateauMed.Infrastructure.DTO;
using PlateauMed.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Infrastructure.Interfaces.Repositories
{
    public interface IProviderRepository
    {
        //add provider
        Task<ProviderModel> AddProvider(ProviderModel provider, Guid userId);

        //set availability
        Task<ProviderModel> SetProviderAvailability(ProviderAvailabilityDTO model, Guid providerId);

        //get by providerId
        Task<ProviderModel> GetProvider(Guid providerId);
        Task<List<ProviderModel>> GetProviders();
        Task<ProviderModel> GetProviderByUserId(Guid userId);
    }
}
