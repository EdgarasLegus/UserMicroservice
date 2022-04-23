using EasyNetQ;
using EasyNetQ.Topology;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserApi.Events.Abstractions;

namespace UserApi.BusinessLogic.Communication
{
    public class MessageBusService : IMessageBusService
    {
        private readonly IBus _bus;
        public MessageBusService(IBus bus)
        {
            _bus = bus;
        }

        public async Task SendMessage<TMessage>(TMessage message, string exchange, string queue) where TMessage : OperationEvent
        {
            var routingKey = "userapi_userCreation";
            var wrappedQueue = _bus.Advanced.QueueDeclare(queue);
            var wrappedExchange = _bus.Advanced.ExchangeDeclare(exchange, ExchangeType.Topic);
            //var binding = _bus.Advanced.Bind(wrappedExchange, wrappedQueue, routingKey);

            var wrappedMessage = new Message<string>(JsonConvert.SerializeObject(message));

            await _bus.Advanced.PublishAsync(wrappedExchange, routingKey, false, wrappedMessage);
            //var yourMessage = new Message<string>(JsonConvert.SerializeObject(new MessageA { Text = "Hello World" }));
            //bus.Publish(Exchange.GetDefault(), queueName, false, false, message);
            //bus.Advanced.Publish<string>(new Exchange("YourExchangeName"), "your.routing.key", false, false, yourMessage);
        }


    }
}
