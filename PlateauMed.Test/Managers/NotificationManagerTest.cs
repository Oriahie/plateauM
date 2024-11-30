using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using PlateauMed.Core;
using PlateauMed.Infrastructure.Exceptions;
using PlateauMed.Infrastructure.Interfaces.Repositories;
using PlateauMed.Infrastructure.Interfaces.Services;
using PlateauMed.Infrastructure.Managers;
using PlateauMed.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Test.Managers
{
    public class NotificationManagerTest
    {
        private readonly INotificationLogRepository _notificationLogRepository;
        private readonly IEmailService _emailService;
        private readonly IPublisher _publisher;
        private readonly NotificationManager _notificationManager;

        public NotificationManagerTest()
        {
            _notificationLogRepository = Substitute.For<INotificationLogRepository>();
            _emailService = Substitute.For<IEmailService>();
            _publisher = Substitute.For<IPublisher>();
            _notificationManager = new NotificationManager(_notificationLogRepository, _emailService, _publisher);
        }


        [Fact]
        public async Task Notify_ShouldAddNotificationAndPublishMessage()
        {
            // Arrange
            var notificationModel = new NotificationLogModel
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                Message = "Test Message",
                Type = NotificationType.AppointmentConfirmation,
                Status = NotificationStatus.NotDelivered
            };
            _notificationLogRepository.AddNotification(notificationModel).Returns(true);

            // Act
            await _notificationManager.Notify(notificationModel);

            // Assert
            await _notificationLogRepository.Received(1).AddNotification(notificationModel);
            await _publisher.Received(1).Publish("true");
        }

        [Fact]
        public async Task Notify_MultipleNotifications_ShouldCallNotifyForEach()
        {
            // Arrange
            var notifications = new List<NotificationLogModel>
            {
                new() { Id = Guid.NewGuid(), Email = "test1@example.com", Message = "Message 1", Type = NotificationType.AppointmentConfirmation },
                new() { Id = Guid.NewGuid(), Email = "test2@example.com", Message = "Message 2", Type = NotificationType.AppointmentConfirmation }
            };

            // Act
            await _notificationManager.Notify(notifications);

            // Assert
            await _notificationLogRepository.Received(notifications.Count).AddNotification(Arg.Any<NotificationLogModel>());
            await _publisher.Received(notifications.Count).Publish(Arg.Any<string>());
        }

        [Fact]
        public async Task ProcessNotification_ShouldSendEmailAndUpdateStatus()
        {
            // Arrange
            var notificationModel = new NotificationLogModel
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                Message = "Test Message",
                Type = NotificationType.AppointmentConfirmation,
                Status = NotificationStatus.NotDelivered
            };

            _emailService.SendEmail(notificationModel.Email, notificationModel.Message, notificationModel.Type.ToString()).Returns(true);

            // Act
            await _notificationManager.ProcessNotification(notificationModel);

            // Assert
            await _emailService.Received(1).SendEmail(notificationModel.Email, notificationModel.Message, notificationModel.Type.ToString());
            await _notificationLogRepository.Received(1).UpdateNotifcation(notificationModel.Id, NotificationStatus.Delivered);
        }

        [Fact]
        public async Task ProcessNotification_ShouldThrowBadRequestException_WhenEmailFails()
        {
            // Arrange
            var notificationModel = new NotificationLogModel
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                Message = "Test Message",
                Type = NotificationType.AppointmentConfirmation,
                Status = NotificationStatus.NotDelivered
            };

            _emailService.SendEmail(notificationModel.Email, notificationModel.Message, notificationModel.Type.ToString()).Returns(false);

            // Act
            var act = async () => await _notificationManager.ProcessNotification(notificationModel);

            // Assert
            await act.Should().ThrowAsync<BadRequestException>().WithMessage("Email failed to send");
            await _notificationLogRepository.DidNotReceive().UpdateNotifcation(notificationModel.Id, notificationModel.Status);
        }
    }
}
