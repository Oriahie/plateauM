using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using PlateauMed.Infrastructure.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Infrastructure.Services
{
    public class Publisher : IPublisher
    {

        private readonly ServiceBusClient _client;
        private readonly IConfiguration _configuration;

        public Publisher(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new ServiceBusClient(_configuration["ServiceBus:ConnectionString"]);
        }

        public async Task<ServiceBusReceiver> DeadLetterReceiverFromTopic()
        {
            var topic = _configuration["ServiceBus:Topic"];
            var subscription = _configuration["ServiceBus:Subscription"];
            ServiceBusReceiver receiver = _client.CreateReceiver(topic, subscriptionName: subscription, options: new ServiceBusReceiverOptions
            {
                SubQueue = SubQueue.DeadLetter,
                ReceiveMode = ServiceBusReceiveMode.PeekLock
            });
            return receiver;
        }
               
        public async Task ProcessDeadLetterMessages(ServiceBusReceiver receiver, CancellationToken cancellationToken)
        {
            try
            {
                var name = _configuration["ServiceBus:Topic"];
                var deadletterMessage = await receiver.ReceiveMessageAsync(cancellationToken: cancellationToken);
                if (deadletterMessage != null)
                {
                    var message = new ServiceBusMessage(deadletterMessage);
                    ServiceBusSender sender = _client.CreateSender(name);
                    await sender.SendMessageAsync(message, cancellationToken);
                    await receiver.CompleteMessageAsync(deadletterMessage, cancellationToken);
                }
            }
            catch (Exception) { }
        }
               
        public Task Publish(string raw)
        {
            var name = _configuration["ServiceBus:Topic"];
            ServiceBusSender sender = _client.CreateSender(name);
            ServiceBusMessage newMessage = new(raw);
            return sender.SendMessageAsync(newMessage);
        }
               
        public async Task<ServiceBusReceiver> ReceiveMessageFromTopic()
        {
            var topic = _configuration["ServiceBus:Topic"];
            var subscription = _configuration["ServiceBus:Subscription"];
            return _client.CreateReceiver(topic, subscriptionName: subscription);
        }
    }
}
