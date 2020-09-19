using ISUCorp.Services.Resources.Requests;
using ISUCorp.Services.Resources.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISUCorp.Services.Contracts.Services
{
    public interface IContactService
    {
        Task<DataResponse<ContactResource>> FindByIdAsync(int id);
        Task<DataResponse<ContactResource>> FindByNameAsync(string name);
        PagedResponse<List<ContactResource>> ListAsync(QueryResource queryResource);
        Task<DataResponse<ContactResource>> AddAsync(SaveContactResource contactResource);
        Task<DataResponse<ContactResource>> UpdateAsync(int contactId, SaveContactResource contactResource);
        Task<YesNoResponse> RemoveAsync(int contactId);
    }
}
