using System.Threading.Tasks;

namespace ISUCorp.Infra.Contracts
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}
