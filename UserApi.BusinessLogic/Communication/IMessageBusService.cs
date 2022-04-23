using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserApi.Events.Abstractions;

namespace UserApi.BusinessLogic.Communication
{
    public interface IMessageBusService
    {
        Task SendMessage<TMessage>(TMessage message, string exchange, string queue) where TMessage : OperationEvent;
    }
}
