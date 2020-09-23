using ISUCorp.Core.Extensions;
using ISUCorp.Services.Contracts.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ISUCorp.Services.Resources.Requests
{
    public class SaveContactResource : ValidatableResource
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        [MaxLength(15)]
        public string Phone { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime BirthDate { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var contactTypes = ContactTypeExtension.GetItems();
            var enterType = contactTypes.Where(type => type.Name == Type).FirstOrDefault();

            if (enterType == null)
            {
                yield return new ValidationResult(
                  $"{nameof(Type)} is incorrect.", new[] { nameof(Type) });
            }
        }
    }
}
