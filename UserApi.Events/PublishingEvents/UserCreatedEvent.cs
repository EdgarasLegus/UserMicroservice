﻿using System;
using System.Collections.Generic;
using System.Text;
using EasyNetQ;
using UserApi.Events.Abstractions;

namespace UserApi.Events.PublishingEvents
{
    //[Queue("userapi_userCreation", ExchangeName = "UserCreationExchange")]
    public class UserCreatedEvent : OperationEvent
    {
        public int UserId { get; set; }
        public string Email { get; set; }

        public UserCreatedEvent(int userId, string email)
        {
            UserId = userId;
            Email = email;
        }
    }
}
