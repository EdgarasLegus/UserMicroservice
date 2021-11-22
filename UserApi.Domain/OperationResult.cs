using System;
using System.Collections.Generic;
using System.Text;

namespace UserApi.Domain
{
    public class OperationResult<T>
    {
        public Status Status { get; set; } = Status.Success;
        public ValidationResult ValidationResult { get; set; } = new ValidationResult();
    }
}
