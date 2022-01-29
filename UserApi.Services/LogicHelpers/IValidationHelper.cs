using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserApi.Domain;
using UserApi.Domain.Entities;

namespace UserApi.Services.LogicHelpers
{
    public interface IValidationHelper
    {
        OperationResult<User> AddExistingUserValidationError(OperationResult<User> operationResult);
        OperationResult<User> AddUserNotFoundValidationError(OperationResult<User> operationResult);
    }
}
