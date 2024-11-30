using PlateauMed.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Infrastructure.Models
{
    public class NotificationLogModel
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }
        public NotificationType Type { get; set; }
        public NotificationStatus Status { get; set; }
    }
}
