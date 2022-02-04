using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UserApi.BusinessLogic.Communication
{
    public interface IMessageBusService
    {
        Task SendMessage<TMessage>(TMessage message) where TMessage : class;
    }
}
