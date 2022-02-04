using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UserApi.Events.Abstractions
{
    public interface IEventHandler<TOperationEvent> where TOperationEvent : OperationEvent
    {
        Task Handle(TOperationEvent operationEvent);
    }
}
