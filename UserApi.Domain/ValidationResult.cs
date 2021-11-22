using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserApi.Domain
{
    public class ValidationResult
    {
        public Dictionary<string, List<string>> PropertyValidations = new Dictionary<string, List<string>>();
    }
}
