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
            await _messageBusService.SendMessage(userCreatedEvent);
        }

        public async Task SendWhenUserDeleted(int userId)
        {
            var userDeletedEvent = new UserDeletedEvent(userId);
            await _messageBusService.SendMessage(userDeletedEvent);
        }
    }
}
