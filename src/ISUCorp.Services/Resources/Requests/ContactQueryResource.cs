using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ISUCorp.Services.Resources.Requests
{
    public class ContactQueryResource : QueryResource
    {
        protected override HashSet<string> AvailableSortOrders => 
            new HashSet<string> { "addedat", "name", "birthdate" };

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext);
        }
    }
}
