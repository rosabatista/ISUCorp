using ISUCorp.Core.Kernel;
using System;
using System.Collections.Generic;

namespace ISUCorp.Core.Domain
{
    public class Contact : BaseEntity
    {
        /// <summary>
        /// Name of the contact.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Contact type.
        /// </summary>
        public ContactType Type { get; set; }

        /// <summary>
        /// Phone of the contact.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Birth date of the contact.
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Reservations made for the contact.
        /// </summary>
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
