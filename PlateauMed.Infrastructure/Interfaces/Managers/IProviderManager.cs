using PlateauMed.Infrastructure.DTO;
using PlateauMed.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Infrastructure.Interfaces.Managers
{
    public interface IProviderManager
    {
        Task<ProviderModel> SetAvailability(ProviderAvailabilityDTO model);
        Task<List<ProviderModel>> GetAll(); 
    }
}
