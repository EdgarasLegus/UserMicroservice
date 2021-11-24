using System;
using System.Collections.Generic;
using System.Text;
using EasyNetQ;

namespace UserApi.Domain.Messages
{
    [Queue("UserQueue", ExchangeName = "UserExchange")]
    public class UserCreatedMessage
    {
        public Guid MessageGuid { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
    }
}
