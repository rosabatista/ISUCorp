using ISUCorp.Core.Domain;
using ISUCorp.Services.Resources.Responses;
using System.Collections.Generic;

namespace ISUCorp.Services.Contracts.Services
{
    public interface IUtilService
    {
        DataResponse<List<ContactTypeItem>> GetContactTypes();
    }
}
