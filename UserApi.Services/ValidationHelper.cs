﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserApi.Domain;
using UserApi.Domain.Entities;

namespace UserApi.Services
{
    public class ValidationHelper : IValidationHelper
    {
        public async Task<OperationResult<User>> AddExistingUserValidationError(OperationResult<User> operationResult)
        {
            operationResult.ValidationResult.PropertyValidations
                .Add(nameof(User.Email), new List<string> { "A user with same email already exists!" });
            operationResult.Status = Status.ValidationError;
            return operationResult;
        }

        public async Task<OperationResult<User>> AddUserNotFoundValidationError(OperationResult<User> operationResult)
        {
            operationResult.ValidationResult.PropertyValidations
                .Add(nameof(User.Email), new List<string> { "A user with this id does not exists!" });
            operationResult.Status = Status.NotFound;
            return operationResult;
        }
    }
}
