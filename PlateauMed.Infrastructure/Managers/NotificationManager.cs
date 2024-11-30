using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PlateauMed.Core;
using PlateauMed.Infrastructure.Exceptions;
using PlateauMed.Infrastructure.Interfaces.Managers;
using PlateauMed.Infrastructure.Interfaces.Repositories;
using PlateauMed.Infrastructure.Interfaces.Services;
using PlateauMed.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PlateauMed.Infrastructure.Managers
{
    public class NotificationManager : INotificationManager
    {
        private readonly INotificationLogRepository _notificationLogRepository;
        private readonly IPublisher _publisher;
        private readonly IEmailService _emailService;

        public NotificationManager(INotificationLogRepository notificationLogRepository, 
                                   IEmailService emailService, 
                                   IPublisher publisher)
        {
            _notificationLogRepository = notificationLogRepository;
            _emailService = emailService;
            _publisher = publisher;
        }

        public async Task Notify(NotificationLogModel model)
        {
            var notification = await _notificationLogRepository.AddNotification(model);
            var payload = JsonConvert.SerializeObject(notification);
            await _publisher.Publish(payload);
        }

        public async Task Notify(List<NotificationLogModel> model)
        {
            foreach (var notification in model)
            {
                await Notify(notification);
            }
        }

        public async Task ProcessNotification(NotificationLogModel model)
        {
            var emailSent = await _emailService.SendEmail(model.Email, model.Message, model.Type.ToString());
            if (!emailSent) throw new BadRequestException("Email failed to send");

            await _notificationLogRepository.UpdateNotifcation(model.Id, NotificationStatus.Delivered);
        }
    }
}
