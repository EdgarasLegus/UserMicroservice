using System.Collections.Generic;
using System.Threading.Tasks;
using UserApi.Domain;
using UserApi.Domain.Entities;

namespace UserApi.Services
{
    public interface IUserService
    {
        IEnumerable<User> GetUsers();
        Task<User> GetUserById(int userId);
        Task<OperationResult<User>> CreateUser(User user);
        Task<OperationResult<User>> DeleteUser(int userId);
    }
}
