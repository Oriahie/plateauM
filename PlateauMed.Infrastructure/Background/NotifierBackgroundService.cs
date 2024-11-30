using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PlateauMed.Infrastructure.Interfaces.Managers;
using PlateauMed.Infrastructure.Interfaces.Services;
using PlateauMed.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlateauMed.Infrastructure.Background
{
    public class NotifierBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IPublisher _publisher;
        private readonly INotificationManager _notificationManager;
        private ServiceBusReceiver _receiver;
        private ServiceBusReceiver _deadLetterReceiver;


        public NotifierBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            var scope = _serviceProvider.CreateScope();
            _publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();
            _notificationManager = scope.ServiceProvider.GetRequiredService<INotificationManager>();
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var successful = await Init();
            if (!successful)
                return;

            while (!stoppingToken.IsCancellationRequested)
            {
                await ProcessMessages(stoppingToken);

                //resend messages in deadletter queue
                await _publisher.ProcessDeadLetterMessages(_deadLetterReceiver, stoppingToken);

            }
        }


        private async Task<bool> Init()
        {
            try
            {
                _receiver = await _publisher.ReceiveMessageFromTopic();

                _deadLetterReceiver = await _publisher.DeadLetterReceiverFromTopic();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task ProcessMessages(CancellationToken cancellationToken)
        {
            try
            {
                var message = await _receiver.ReceiveMessageAsync(cancellationToken: cancellationToken);
                if (message != null)
                {
                    var body = message.Body.ToString();
                    if (string.IsNullOrEmpty(body))
                    {
                        return;
                    }
                    
                    var request = JsonSerializer.Deserialize<NotificationLogModel>(body);

                    await _notificationManager.ProcessNotification(request);

                    await _receiver.CompleteMessageAsync(message);
                }
            }
            catch (Exception)
            { }
        }

    }
}
