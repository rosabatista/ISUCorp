using ISUCorp.Services.Resources.Models;
using ISUCorp.Services.Resources.Requests;
using ISUCorp.Services.Resources.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISUCorp.Services.Contracts.Services
{
    public interface IPlaceService
    {
        Task<DataResponse<PlaceResource>> FindByIdAsync(int id);
        Task<DataResponse<PlaceResource>> FindByNameAsync(string name);
        PagedResponse<List<PlaceResource>> ListAsync(QueryResource queryResource);
        Task<DataResponse<PlaceResource>> AddAsync(SavePlaceResource placeResource);
        Task<DataResponse<PlaceResource>> UpdateAsync(int placeId, SavePlaceResource placeResource);
        Task<YesNoResponse> RemoveAsync(int contactId);
    }
}
