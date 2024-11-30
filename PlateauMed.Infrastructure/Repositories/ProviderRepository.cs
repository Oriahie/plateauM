using Microsoft.EntityFrameworkCore;
using PlateauMed.Core;
using PlateauMed.Core.Entities;
using PlateauMed.Infrastructure.DTO;
using PlateauMed.Infrastructure.Exceptions;
using PlateauMed.Infrastructure.Interfaces.Repositories;
using PlateauMed.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Infrastructure.Repositories
{
    public class ProviderRepository : IProviderRepository
    {
        private readonly DataDbContext _dbContext;

        public ProviderRepository(DataDbContext dbContext) => _dbContext = dbContext;
        public async Task<ProviderModel> AddProvider(ProviderModel provider, Guid userId)
        {
            var entity = provider.Map(userId);
            await _dbContext.Set<Provider>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity.Map();
        }
               
        public async Task<ProviderModel> GetProvider(Guid providerId)
        {
            var record = await _dbContext.Set<Provider>()
                                         .AsNoTracking()
                                         .Where(x=>x.Id == providerId)
                                         .Select(x=>x.Map())
                                         .FirstOrDefaultAsync() ??
                         throw new NotFoundException("Provider not found");
            return record;
        }

        public async Task<ProviderModel> GetProviderByUserId(Guid userId)
        {

            var record = await _dbContext.Set<Provider>()
                                         .AsNoTracking()
                                         .Where(x => x.UserId == userId)
                                         .Select(x => x.Map())
                                         .FirstOrDefaultAsync() ??
                         throw new NotFoundException("Provider not found");
            return record;
        }

        public async Task<List<ProviderModel>> GetProviders()
        {
            return await _dbContext.Set<Provider>()
                                   .AsNoTracking()
                                   .OrderByDescending(x => x.CreatedDate)
                                   .Select(x => x.Map())
                                   .ToListAsync();
        }

        public async Task<ProviderModel> SetProviderAvailability(ProviderAvailabilityDTO model, Guid providerId)
        {
            var record = await _dbContext.Set<Provider>()
                                         .FirstOrDefaultAsync(x => x.Id == providerId) ??
                         throw new NotFoundException("Provider not found");

            record.StartTime = model.StartTime;
            record.CloseTime = model.CloseTime;
            record.BreakStartTime = model.BreakStartTime;
            record.BreakDurationInMiniutes = model.BreakDuration;
            record.ModifiedDate = DateTimeOffset.Now;

            await _dbContext.SaveChangesAsync();
            return record.Map();
        }
    }
}
