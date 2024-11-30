using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Infrastructure.DTO
{
    public class ProviderAvailabilityDTO
    {
        [Required]
        public TimeSpan StartTime { get; set; }
        [Required]
        public TimeSpan CloseTime { get; set; }
        [Required]
        public TimeSpan BreakStartTime { get; set; }
        [Required]
        [Range(1,5)]
        public long BreakDuration { get; set; }
    }
}
