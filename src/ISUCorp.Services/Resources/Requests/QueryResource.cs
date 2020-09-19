using ISUCorp.Services.Contracts.Resources;
using Microsoft.EntityFrameworkCore.Internal;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ISUCorp.Services.Resources.Requests
{
    public class QueryResource : ValidatableResource
    {
        [Required]
        public int PageNumber { get; set; }

        [Required]
        [Range(1, 50)]
        public int PageSize { get; set; }

        public string SortOrder { get; set; }

        public string SearchBy { get; set; }

        protected virtual HashSet<string> AvailableSortOrders => null;

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PageNumber < 1)
            {
                yield return new ValidationResult(
                  $"{nameof(PageNumber)} is incorrect.", new[] { nameof(PageNumber) });
            }

            if (PageSize < 1)
            {
                yield return new ValidationResult(
                  $"{nameof(PageSize)} is incorrect.", new[] { nameof(PageSize) });
            }

            if (!string.IsNullOrWhiteSpace(SortOrder) && 
                !AvailableSortOrders.Where(e => SortOrder.Trim().ToLower().StartsWith(e.ToLower())).Any())
            {
                yield return new ValidationResult(
                  $"{nameof(SortOrder)} is incorrect.", new[] { nameof(SortOrder) });
            }
        }
    }
}
