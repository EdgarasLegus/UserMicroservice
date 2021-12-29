using System;
using System.Collections.Generic;
using System.Text;
using EasyNetQ;

namespace UserApi.Messages
{
    //[Queue("user_creation", ExchangeName = "UserCreationExchange")]
    public class UserCreatedMessage
    {
        public Guid MessageGuid { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
    }
}
