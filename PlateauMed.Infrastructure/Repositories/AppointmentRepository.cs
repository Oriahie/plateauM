using Microsoft.EntityFrameworkCore;
using PlateauMed.Core;
using PlateauMed.Core.Entities;
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
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly DataDbContext _dbContext;

        public AppointmentRepository(DataDbContext dbContext) => _dbContext = dbContext;

        public async Task<AppointmentModel> CreateAppointment(AppointmentModel model)
        {
            var entity = model.Map();
            await _dbContext.Set<Appointment>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity.Map();
        }

        public async Task<AppointmentModel> GetAppointment(Guid appointmentId)
        {
            var record = await _dbContext.Set<Appointment>()
                                         .AsNoTracking()
                                         .Where(x => x.Id == appointmentId)
                                         .Select(x => x.Map())
                                         .FirstOrDefaultAsync() ??
                         throw new NotFoundException("Appointment not found");
            return record;
        }

        public async Task<List<AppointmentModel>> GetAppointments(FilterParam filterParam, string arg)
        {
            var query = _dbContext.Set<Appointment>().AsNoTracking();

            query = filterParam switch
            {
                FilterParam.Provider => Guid.TryParse(arg, out var provider) ? query.Where(x => x.ProviderId == provider) : query,
                FilterParam.Date => DateTimeOffset.TryParse(arg, out var date) ? query.Where(x => x.AppointmentDate == date) : query,
                FilterParam.Patient => Guid.TryParse(arg, out var patient) ? query.Where(x => x.PatientId == patient) : query,
                _ => query
            };

            var records = await query.OrderByDescending(x => x.CreatedDate)
                                     .Select(x => x.Map())
                                     .ToListAsync();

            return records;
        }

        public async Task<List<AppointmentModel>> GetAppointments(Guid providerId)
        {
            return await _dbContext.Set<Appointment>()
                                   .AsNoTracking()
                                   .Where(x => x.ProviderId == providerId)
                                   .Select(x => x.Map())
                                   .OrderByDescending(x => x.CreatedDate)
                                   .ToListAsync();
        }

        public async Task<AppointmentMetricsModel> GetMetrics(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var records = _dbContext.Set<Appointment>()
                                          .AsNoTracking()
                                          .Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate);
            return new AppointmentMetricsModel()
            {
                CompletedAppointments = await records.CountAsync(x => x.Status == AppointmentStatus.Completed),
                PendingAppointments = await records.CountAsync(x => x.Status == AppointmentStatus.Pending),
                TotalAppointments = await records.CountAsync(),
                TotalCancellations = await records.CountAsync(x => x.Status == AppointmentStatus.Cancelled)
            };
        }

        public async Task<(string, string)> GetProviderPatientEmail(Guid appointmentId)
        {
            var record = await _dbContext.Set<Appointment>()
                                          .AsNoTracking()
                                          .Where(x => x.Id == appointmentId)
                                          .Select(x => new
                                          {
                                              PatientEmail = x.Patient.User.Email,
                                              ProviderEmail = x.Provider.User.Email
                                          })
                                          .FirstOrDefaultAsync() ??
                          throw new NotFoundException("Appointment not found");
            return (record.PatientEmail, record.ProviderEmail);
        }

        public async Task<AppointmentModel> UpdateAppointmentStatus(Guid appointmentId, AppointmentStatus status)
        {
            var appointment = await _dbContext.Set<Appointment>()
                                              .FirstOrDefaultAsync(x => x.Id == appointmentId) ??
                              throw new NotFoundException("Appointment not Found");

            appointment.Status = status;
            appointment.ModifiedDate = DateTimeOffset.Now;
            await _dbContext.SaveChangesAsync();
            return appointment.Map();
        }


        public async Task<bool> HasOverlappingAppointments(Guid providerId, DateTimeOffset appointmentDate)
        {
            return await _dbContext.Set<Appointment>()
                                   .AsNoTracking()
                                   .Where(x => x.ProviderId == providerId &&
                                               x.AppointmentDate == appointmentDate)
                                   .AnyAsync();
        }
    }
}
