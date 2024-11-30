using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Core.Entities
{
    public class Provider : BaseEntity
    {
        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan CloseTime { get; set; }
        public TimeSpan BreakStartTime { get; set; }
        public long BreakDurationInMiniutes { get; set; }
    }
}
