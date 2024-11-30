using PlateauMed.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Infrastructure.DTO
{
    public class AppointmentDTO
    {
        [Required]
        public Guid ProviderId { get; set; }
        public string Reason { get; set; }
        [Required]
        public DateTimeOffset AppointmentDate { get; set; }
    }
}
