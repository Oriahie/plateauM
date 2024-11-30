using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Infrastructure.Interfaces.Services
{
    public interface IPublisher
    {
        Task Publish(string raw);
        Task<ServiceBusReceiver> ReceiveMessageFromTopic();
        Task<ServiceBusReceiver> DeadLetterReceiverFromTopic();
        Task ProcessDeadLetterMessages(ServiceBusReceiver receiver, CancellationToken cancellationToken);

    }
}
