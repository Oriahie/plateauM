using PlateauMed.Core;
using PlateauMed.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Infrastructure.Interfaces.Repositories
{
    public interface IAppointmentRepository
    {
        //create appointment
        Task<AppointmentModel> CreateAppointment(AppointmentModel model);

        //update appointment status
        Task<AppointmentModel> UpdateAppointmentStatus(Guid appointmentId, AppointmentStatus status);

        Task<(string, string)> GetProviderPatientEmail(Guid appointmentId);

        //get appoint by id
        Task<AppointmentModel> GetAppointment(Guid appointmentId);

        //get list of appointments by providerId,date,patientId
        Task<List<AppointmentModel>> GetAppointments(FilterParam filterParam, string arg);
        Task<List<AppointmentModel>> GetAppointments(Guid providerId);

        //get metrics
        Task<AppointmentMetricsModel> GetMetrics(DateTimeOffset startDate, DateTimeOffset endDate);
        Task<bool> HasOverlappingAppointments(Guid providerId, DateTimeOffset appointmentDate);
    }
}
