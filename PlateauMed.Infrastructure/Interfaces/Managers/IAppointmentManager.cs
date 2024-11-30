using PlateauMed.Core;
using PlateauMed.Infrastructure.DTO;
using PlateauMed.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Infrastructure.Interfaces.Managers
{
    public interface IAppointmentManager
    {
        //get report
        Task<AppointmentMetricsModel> GetAppointmentMetrics(DateTimeOffset startDate, DateTimeOffset endDate);
        Task<List<AppointmentModel>> Get(FilterParam filter, string query);
        Task<List<AppointmentModel>> GetForProvider();
        Task<List<AppointmentModel>> GetForPatient();
        Task<AppointmentModel> UpdateStatus(Guid appointmentId, AppointmentStatus status);
        Task<AppointmentModel> CancelAppointment(Guid appointmentId);
        Task<AppointmentModel> BookAppointment(AppointmentDTO model);
    }
}
