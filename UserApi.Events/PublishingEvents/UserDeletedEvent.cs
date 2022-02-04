using System;
using System.Collections.Generic;
using System.Text;
using UserApi.Events.Abstractions;
using EasyNetQ;

namespace UserApi.Events.PublishingEvents
{
    [Queue("userapi_userexport", ExchangeName = "UserDeletedExchange")]
    public class UserDeletedEvent : OperationEvent
    {
        public int UserId { get; set; }

        public UserDeletedEvent(int userId)
        {
            UserId = userId;
        }
    }
}
