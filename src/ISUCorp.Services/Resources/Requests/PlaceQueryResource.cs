using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ISUCorp.Services.Resources.Requests
{
    public class PlaceQueryResource : QueryResource
    {
        protected override HashSet<string> AvailableSortOrders =>
            new HashSet<string> { "addedat", "name" };

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext);
        }
    }
}
