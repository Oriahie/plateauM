using PlateauMed.Core;
using PlateauMed.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Infrastructure.Interfaces.Repositories
{
    public interface INotificationLogRepository
    {
        Task<bool> AddNotification(NotificationLogModel notificationLog);
        Task<bool> UpdateNotifcation(Guid notificationId, NotificationStatus status);
    }
}
