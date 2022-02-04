using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserApi.Domain.Entities;

namespace UserApi.BusinessLogic.EventServices
{
    public interface IUserEventService
    {
        Task SendCreatedUserInformation(User user);
        Task SendWhenUserDeleted(int userId);
    }
}
