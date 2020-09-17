using ISUCorp.Core.Kernel;
using System.Collections.Generic;

namespace ISUCorp.Core.Domain
{
    public class Place : BaseEntity
    {
        /// <summary>
        /// Name of the place.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the place.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Reservations made at this place.
        /// </summary>
        public virtual ICollection<Reservation> Reservations { get; set;}
    }
}
