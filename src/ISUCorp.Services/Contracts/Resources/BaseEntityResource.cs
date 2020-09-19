using System;

namespace ISUCorp.Services.Contracts.Resources
{
    public abstract class BaseEntityResource
    {
        /// <summary>
        /// Entity identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Time when the entity was added at.
        /// </summary>
        public DateTime AddedAt { get; set; }

        /// <summary>
        /// Time when the entity was modified at.
        /// </summary>
        public DateTime? ModifiedAt { get; set; }
    }
}
