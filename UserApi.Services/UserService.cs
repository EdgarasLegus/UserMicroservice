using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserApi.Data.DataAccess;
using UserApi.Domain;
using UserApi.Domain.Entities;
using UserApi.Domain.Messages;

namespace UserApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<User> _userRepository;
        private readonly IValidationHelper _validationHelper;
        private readonly IMessageBusService _messageBusService;
        public UserService(IUnitOfWork unitOfWork, IValidationHelper validationHelper, 
            IMessageBusService messageBusService)
        {
            _unitOfWork = unitOfWork;
            _validationHelper = validationHelper;
            _messageBusService = messageBusService;
            _userRepository = _unitOfWork.GetRepository<User>();
        }

        public IEnumerable<User> GetUsers()
        {
            return _userRepository.Get();
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _userRepository.GetFirstOrDefault(x => x.UserId == userId);
        }

        public async Task<OperationResult<User>> CreateUser(User user)
        {
            var operationResult = new OperationResult<User>();
            var existingUserWithSameEmail = await _userRepository.GetFirstOrDefault(u => u.Email.Equals(user.Email));

            if (existingUserWithSameEmail == null)
            {
                await _userRepository.Insert(user);
                await _unitOfWork.CommitAsync();
                await _messageBusService.SendMessage(new UserCreatedMessage
                {
                    MessageGuid = new Guid(),
                    UserId = user.UserId,
                    Email = user.Email
                });

                return operationResult;
            }
            return _validationHelper.AddExistingUserValidationError(operationResult);
        }

        public async Task<OperationResult<User>> DeleteUser(int userId)
        {
            var operationResult = new OperationResult<User>();
            var existingUser = await _userRepository.GetFirstOrDefault(u => u.UserId.Equals(userId));

            if (existingUser != null)
            {
                _userRepository.Delete(existingUser);
                await _unitOfWork.CommitAsync();

                return operationResult;
            }
            return _validationHelper.AddUserNotFoundValidationError(operationResult);
        }
    }
}
