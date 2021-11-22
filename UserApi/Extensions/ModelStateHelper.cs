using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserApi.Domain;

namespace UserApi.Extensions
{
    public static class ModelStateHelper
    {
        public static ModelStateDictionary ToModelState(this ValidationResult validationResult)
        {
            var modelState = new ModelStateDictionary();

            if (!validationResult.PropertyValidations.Any())
            {
                return modelState;
            }
            return AddPropertyValidationErrors(validationResult, modelState);
        }

        private static ModelStateDictionary AddPropertyValidationErrors(ValidationResult validationResult, ModelStateDictionary modelState)
        {
            foreach (var pv in validationResult.PropertyValidations)
            {
                foreach (var msg in pv.Value)
                {
                    modelState.AddModelError(pv.Key, msg);
                }
            }
            return modelState;
        }
    }
}
