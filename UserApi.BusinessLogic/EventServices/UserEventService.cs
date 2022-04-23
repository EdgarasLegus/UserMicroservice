using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UserApi.BusinessLogic.Communication;
using UserApi.Domain.Entities;
using UserApi.Events.PublishingEvents;

namespace UserApi.BusinessLogic.EventServices
{
    public class UserEventService : IUserEventService
    {
        private readonly IMessageBusService _messageBusService;

        public UserEventService(IMessageBusService messageBusService)
        {
            _messageBusService = messageBusService;
        }

        public async Task SendCreatedUserInformation(User user)
        {
            var userCreatedEvent = new UserCreatedEvent(user.UserId, user.Email);
            //var one = userCreatedEvent.GetType();
            //var two = one.GetProperties();
            //var three = two.Select(x => x.GetCustomAttribute<EasyNetQ.QueueAttribute>());
            //var four = three.Select(x => x.QueueName);
            //var five = four.FirstOrDefault();

            //var exchange = userCreatedEvent.GetType()
            //    .GetProperties()
            //    .Select(x => x.GetCustomAttribute<EasyNetQ.QueueAttribute>())
            //    .Select(x => x.ExchangeName)
            //    .FirstOrDefault();

            //var queue = userCreatedEvent.GetType()
            //    .GetProperties()
            //    .Select(x => x.GetCustomAttribute<EasyNetQ.QueueAttribute>())
            //    .Select(x => x.QueueName)
            //    .FirstOrDefault();

            //var userCreatedEventTopic = $"UserId.{userCreatedEvent.UserId}.Email.{userCreatedEvent.Email}";
            await _messageBusService.SendMessage(userCreatedEvent, "UserCreationExchange", "userapi_userCreation");
        }

        public async Task SendWhenUserDeleted(int userId)
        {
            var userDeletedEvent = new UserDeletedEvent(userId);
            //var exchange = userDeletedEvent.GetType()
            //    .GetProperties()
            //    .Select(x => x.GetCustomAttribute<EasyNetQ.QueueAttribute>())
            //    .Where(x => x != null)
            //    .Select(x => x.ExchangeName)
            //    .FirstOrDefault();

            //var queue = userDeletedEvent.GetType()
            //    .GetProperties()
            //    .Select(x => x.GetCustomAttribute<EasyNetQ.QueueAttribute>())
            //    .Where(x => x != null)
            //    .Select(x => x.QueueName)
            //    .FirstOrDefault();

            //var userDeletedEventTopic = $"UserId.{userDeletedEvent.UserId}";
            await _messageBusService.SendMessage(userDeletedEvent, "UserDeletionExchange", "userapi_userDeletion");
        }
    }
}
