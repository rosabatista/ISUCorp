using ISUCorp.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ISUCorp.Core.Extensions
{
    public static class ContactTypeExtension
    {
        public static List<ContactTypeItem> GetItems()
        {
            return Enum.GetValues(typeof(ContactType))
                       .Cast<ContactType>()
                       .Select(type => new ContactTypeItem { Value = (int)type, Name = type.ToString() })
                       .ToList();
        }
    }
}
