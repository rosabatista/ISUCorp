using ISUCorp.Core.Domain;
using ISUCorp.Core.Extensions;
using ISUCorp.Services.Resources.Requests;
using System;
using System.Linq;

namespace ISUCorp.Services.Mappers
{
    public class ContactMapper
    {
        public static void Map(Contact contact, SaveContactResource contactResource)
        {
            if (contact == null || contactResource == null)
            {
                throw new ArgumentNullException("Wrong contact or contact resource provided.");
            }

            var type = Enum.GetValues(typeof(ContactType))
                           .Cast<ContactType>()
                           .Where(t => t.ToString() == contactResource.Type)
                           .FirstOrDefault();

            contact.Name = contactResource.Name;
            contact.Type = type;
            contact.BirthDate = contactResource.BirthDate;
            contact.Phone = contactResource.Phone;
        }
    }
}
