using PlateauMed.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Infrastructure.Models
{
    public class AppointmentModel
    {
        public Guid Id { get; set; }
        public string PatientName { get; set; }
        public Guid PatientId { get; set; }
        public Guid ProviderId { get; set; }
        public string ProviderName { get; set; }
        public string Reason { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset AppointmentDate { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}
