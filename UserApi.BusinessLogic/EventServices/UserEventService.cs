using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UserApi.BusinessLogic.Communication;
using UserApi.BusinessLogic.LogicHelpers;
using UserApi.Domain.Entities;
using UserApi.Events.PublishingEvents;

namespace UserApi.BusinessLogic.EventServices
{
    public class UserEventService : IUserEventService
    {
        private readonly IMessageBusService _messageBusService;
        private readonly IAttributeExtractionHelper _attributeExtractionHelper;

        public UserEventService(IMessageBusService messageBusService, IAttributeExtractionHelper attributeExtractionHelper)
        {
            _messageBusService = messageBusService;
            _attributeExtractionHelper = attributeExtractionHelper;
        }

        public async Task SendCreatedUserInformation(User user)
        {
            var userCreatedEvent = new UserCreatedEvent(user.UserId, user.Email);
            //(var routingkey, var exchange) = _attributeExtractionHelper.GetQueueAttributeNames(userCreatedEvent);
            await _messageBusService.SendMessage(userCreatedEvent, userCreatedEvent.Exchange, userCreatedEvent.RoutingKey);
        }

        public async Task SendWhenUserDeleted(int userId)
        {
            var userDeletedEvent = new UserDeletedEvent(userId);
            //(var routingkey, var exchange) = _attributeExtractionHelper.GetQueueAttributeNames(userDeletedEvent);
            await _messageBusService.SendMessage(userDeletedEvent, "UserCreationExchange", "cartapi_userUpdates");
        }
    }
}
