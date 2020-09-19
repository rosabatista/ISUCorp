using System;
using System.ComponentModel.DataAnnotations;

namespace ISUCorp.Services.Resources.Requests
{
    public class SavePlaceResource
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
