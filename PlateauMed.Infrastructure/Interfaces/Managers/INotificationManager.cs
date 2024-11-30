using PlateauMed.Core;
using PlateauMed.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Infrastructure.Interfaces.Managers
{
    public interface INotificationManager
    {
        Task Notify(NotificationLogModel model);
        Task Notify(List<NotificationLogModel> model);
        Task ProcessNotification(NotificationLogModel model);
    }
}
