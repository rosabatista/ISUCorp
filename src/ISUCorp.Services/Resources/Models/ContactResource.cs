using ISUCorp.Core.Domain;
using ISUCorp.Services.Contracts.Resources;
using System;

namespace ISUCorp.Services.Resources.Responses
{
    /// <summary>
    /// Represents a contact.
    /// </summary>
    public class ContactResource : BaseEntityResource
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
    }
}
