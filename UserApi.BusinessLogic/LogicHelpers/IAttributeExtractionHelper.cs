using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserApi.Events.Abstractions;

namespace UserApi.BusinessLogic.LogicHelpers
{
    public interface IAttributeExtractionHelper
    {
        (string, string) GetQueueAttributeNames(OperationEvent operationEvent);
    }
}
