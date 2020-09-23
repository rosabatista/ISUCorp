using ISUCorp.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISUCorp.Infra.Contracts
{
    public interface IPlaceRepository : IAsyncRepository<Place>
    {
        Task<List<Place>> SearchByName(string term);
    }
}
