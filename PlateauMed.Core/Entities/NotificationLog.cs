using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Core.Entities
{
    public class NotificationLog : BaseEntity
    {
        public string Message { get; set; }
        public string Email { get; set; }
        public NotificationType Type { get; set; }
        public NotificationStatus Status { get; set; }
    }
}
