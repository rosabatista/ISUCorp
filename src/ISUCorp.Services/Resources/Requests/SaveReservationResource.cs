using ISUCorp.Services.Contracts.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ISUCorp.Services.Resources.Requests
{
    public class SaveReservationResource : ValidatableResource
    {
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date { get; set; }

        public string Notes { get; set; }

        [Range(1, 5)]
        public int? Rating { get; set; }

        public bool IsFavorite { get; set; }

        [Required]
        public int PlaceId { get; set; }

        [Required]
        public SaveContactResource Contact { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PlaceId <= 0)
            {
                yield return new ValidationResult(
                  $"{nameof(PlaceId)} is incorrect.", new[] { nameof(PlaceId) });
            }

            if (Date.ToUniversalTime() < DateTime.UtcNow)
            {
                yield return new ValidationResult(
                      $"{nameof(Date)} is incorrect.", new[] { nameof(Date) });
            }
        }
    }
}
