using ISUCorp.Core.Domain;
using ISUCorp.Core.Extensions;
using ISUCorp.Services.Contracts.Services;
using ISUCorp.Services.Resources.Responses;
using System;
using System.Collections.Generic;

namespace ISUCorp.Services.Services
{
    public class UtilService : IUtilService
    {
        public DataResponse<List<ContactTypeItem>> GetContactTypes()
        {
            try
            {
                var contactTypes = ContactTypeExtension.GetItems();
                return new DataResponse<List<ContactTypeItem>>(contactTypes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new DataResponse<List<ContactTypeItem>>(ex.Message);
            }
        }
    }
}
