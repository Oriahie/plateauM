using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Core.Entities
{
    public class Appointment : BaseEntity
    {
        public Guid PatientId { get; set; }
        [ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; }


        public Guid ProviderId { get; set; }
        [ForeignKey(nameof(ProviderId))]
        public Provider Provider { get; set; }

        public string Reason { get; set; }

        public DateTimeOffset AppointmentDate { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}
