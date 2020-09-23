using ISUCorp.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISUCorp.Infra.Contracts
{
    public interface IContactRepository : IAsyncRepository<Contact>
    {
        Task<List<Contact>> SearchByName(string term);
    }
}
