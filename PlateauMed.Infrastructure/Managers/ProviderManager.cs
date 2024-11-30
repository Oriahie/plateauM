using PlateauMed.Infrastructure.DTO;
using PlateauMed.Infrastructure.Exceptions;
using PlateauMed.Infrastructure.Interfaces.Managers;
using PlateauMed.Infrastructure.Interfaces.Repositories;
using PlateauMed.Infrastructure.Interfaces.Services;
using PlateauMed.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Infrastructure.Managers
{
    public class ProviderManager : IProviderManager
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IUtilityService _utilityService;

        public ProviderManager(IProviderRepository providerRepository, IUtilityService utilityService)
        {
            _providerRepository = providerRepository;
            _utilityService = utilityService;
        }

        public async Task<List<ProviderModel>> GetAll()
        {
            return await _providerRepository.GetProviders();
        }

        public async Task<ProviderModel> SetAvailability(ProviderAvailabilityDTO model)
        {
            if (model.StartTime > model.CloseTime)
                throw new ValidationException("Ensure Start time and Close time are correct");

            var breakEnd = model.BreakStartTime.Add(TimeSpan.FromHours(model.BreakDuration));

            if (breakEnd > model.CloseTime)
                throw new ValidationException("Ensure break time does not exceeed closing time");
            
            _ = Guid.TryParse(_utilityService.UserId(), out Guid userId);
            var provider = await _providerRepository.GetProviderByUserId(userId);

            return await _providerRepository.SetProviderAvailability(model, provider.ProviderId);
        }
    }
}
