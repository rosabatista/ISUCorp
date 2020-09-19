using ISUCorp.Core.Domain;
using ISUCorp.Services.Resources.Requests;
using System;

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

            contact.Name = contactResource.Name;
            contact.Type = contactResource.Type;
            contact.BirthDate = contactResource.BirthDate;
            contact.Phone = contactResource.Phone;
        }
    }
}
