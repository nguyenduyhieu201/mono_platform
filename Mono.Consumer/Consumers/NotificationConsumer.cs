using MassTransit;
using Mono.Consumer.Events;

namespace Mono.Consumer.Consumers
{
    public class NotificationConsumer : IConsumer<SendNotificationEvent>
    {
        public Task Consume(ConsumeContext<SendNotificationEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
