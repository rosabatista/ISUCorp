using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ISUCorp.Services.Resources.Requests
{
    public class ReservationQueryResource : QueryResource
    {
        protected override HashSet<string> AvailableSortOrders =>
            new HashSet<string> { "date", "name", "rating" };

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext);
        }
    }
}
