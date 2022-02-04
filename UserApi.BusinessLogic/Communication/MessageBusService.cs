using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UserApi.BusinessLogic.Communication
{
    public class MessageBusService : IMessageBusService
    {
        private readonly IBus _bus;
        public MessageBusService(IBus bus)
        {
            _bus = bus;
        }

        public async Task SendMessage<TMessage>(TMessage message) where TMessage : class
        {
            await _bus.PubSub.PublishAsync(message);
        }


    }
}
