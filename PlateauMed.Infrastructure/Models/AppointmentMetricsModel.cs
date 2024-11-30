using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Infrastructure.Models
{
    public class AppointmentMetricsModel
    {
        public long TotalAppointments { get; set; }
        public long TotalCancellations { get; set; }
        public long CompletedAppointments { get; set; }
        public long PendingAppointments { get; set; }
    }
}
