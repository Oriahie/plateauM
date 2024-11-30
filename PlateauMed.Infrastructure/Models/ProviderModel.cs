using PlateauMed.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Infrastructure.Models
{
    public class ProviderModel
    {
        public Guid ProviderId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string ProviderName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan CloseTime { get; set; }
        public TimeSpan BreakStartTime { get; set; }
        public long BreakDuration { get; set; }
    }
}
