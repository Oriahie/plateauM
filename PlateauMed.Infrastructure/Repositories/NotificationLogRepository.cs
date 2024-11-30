using PlateauMed.Core;
using PlateauMed.Core.Entities;
using PlateauMed.Infrastructure.Exceptions;
using PlateauMed.Infrastructure.Interfaces.Repositories;
using PlateauMed.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Infrastructure.Repositories
{
    public class NotificationLogRepository : INotificationLogRepository
    {
        private readonly DataDbContext _dbContext;

        public NotificationLogRepository(DataDbContext dbContext) => _dbContext = dbContext;

        public async Task<bool> AddNotification(NotificationLogModel notificationLog)
        {
            var entity = notificationLog.Map();
            await _dbContext.Set<NotificationLog>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }
               
        public async Task<bool> UpdateNotifcation(Guid notificationId, NotificationStatus status)
        {
            var record = await _dbContext.Set<NotificationLog>().FindAsync(notificationId) ?? 
                         throw new NotFoundException("Notification Not Found");

            record.Status = status;
            record.ModifiedDate = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
