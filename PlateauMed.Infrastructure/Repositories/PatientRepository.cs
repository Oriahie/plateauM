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
    public class PatientRepository : IPatientRepository
    {
        private readonly DataDbContext _dbContext;

        public PatientRepository(DataDbContext dbContext) => _dbContext = dbContext;

        public async Task<PatientModel> AddPatient(PatientModel patient, Guid UserId)
        {
            var entity = new Patient()
            {
                UserId = UserId,
                DateOfBirth = patient.DateOfBirth
            };
            await _dbContext.Set<Patient>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity.Map();
        }

        public async Task<PatientModel> GetByUserId(Guid userId)
        {
            var record = await _dbContext.Set<Patient>()
                             .AsNoTracking()
                             .Where(x => x.UserId == userId)
                             .Select(x => x.Map())
                             .FirstOrDefaultAsync() ??
             throw new NotFoundException("Provider not found");
            return record;
        }

        public async Task<PatientModel> GetPatient(Guid PatientId)
        {
            var record =  await _dbContext.Set<Patient>()
                                          .AsNoTracking()
                                          .Where(x=>x.Id == PatientId)
                                          .Select(x=>x.Map())
                                          .FirstOrDefaultAsync() ??
                                   throw new NotFoundException("Patient Record Not Found");

            return record;
        }
    }
}
