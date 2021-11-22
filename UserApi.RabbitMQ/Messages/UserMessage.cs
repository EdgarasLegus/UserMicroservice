using System;

namespace UserApi.RabbitMQ.Messages
{
    //[Queue("TestMessagesQueue", ExchangeName = "MyTestExchange")]
    public class UserMessage
    {
        public string Text { get; set; }
    }
}
