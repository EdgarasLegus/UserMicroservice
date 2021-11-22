using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserApi.Domain;
using UserApi.Domain.Entities;

namespace UserApi.Services
{
    public interface IValidationHelper
    {
        Task<OperationResult<User>> AddExistingUserValidationError(OperationResult<User> operationResult);
        Task<OperationResult<User>> AddUserNotFoundValidationError(OperationResult<User> operationResult);
    }
}
