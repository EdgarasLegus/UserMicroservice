using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UserApi.Events.Abstractions;

namespace UserApi.BusinessLogic.LogicHelpers
{
    public class AttributeExtractionHelper : IAttributeExtractionHelper
    {
        public (string, string) GetQueueAttributeNames(OperationEvent operationEvent)
        {
            var routingKey = operationEvent
                .GetType()
                .GetCustomAttribute<EasyNetQ.QueueAttribute>()
                .QueueName;

            var exchange = operationEvent
                .GetType()
                .GetCustomAttribute<EasyNetQ.QueueAttribute>()
                .ExchangeName;

            return (routingKey, exchange);
        }
    }
}
