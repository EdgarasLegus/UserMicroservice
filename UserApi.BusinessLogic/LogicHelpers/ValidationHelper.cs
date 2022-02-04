using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserApi.Domain;
using UserApi.Domain.Entities;

namespace UserApi.BusinessLogic.LogicHelpers
{
    public class ValidationHelper : IValidationHelper
    {
        public OperationResult<User> AddExistingUserValidationError(OperationResult<User> operationResult)
        {
            operationResult.ValidationResult.PropertyValidations
                .Add(nameof(User.Email), new List<string> { "A user with same email already exists!" });
            operationResult.Status = Status.ValidationError;
            return operationResult;
        }

        public OperationResult<User> AddUserNotFoundValidationError(OperationResult<User> operationResult)
        {
            operationResult.ValidationResult.PropertyValidations
                .Add(nameof(User.UserId), new List<string> { "A user with this id does not exists!" });
            operationResult.Status = Status.NotFound;
            return operationResult;
        }
    }
}
