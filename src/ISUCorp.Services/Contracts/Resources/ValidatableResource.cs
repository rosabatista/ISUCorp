using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ISUCorp.Services.Contracts.Resources
{
    public abstract class ValidatableResource : IValidatableObject
    {
        public abstract IEnumerable<ValidationResult> Validate(ValidationContext validationContext);
    }
}
