using ISUCorp.Services.Contracts.Resources;

namespace ISUCorp.Services.Resources.Models
{
    /// <summary>
    /// Represents a place.
    /// </summary>
    public class PlaceResource : BaseEntityResource
    {
        /// <summary>
        /// Name of the place.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the place.
        /// </summary>
        public string Description { get; set; }
    }
}
