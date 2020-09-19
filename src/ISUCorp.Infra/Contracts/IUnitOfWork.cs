using ISUCorp.Core.Kernel;
using ISUCorp.Infra.Contexts;
using System;
using System.Threading.Tasks;

namespace ISUCorp.Infra.Contracts
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }

    public interface IUnitOfWork<TContext> : IDisposable where TContext : CoreDbContext
    {
        TContext DbContext { get; }

        IAsyncRepository<TEntity> GetRepositoryAsync<TEntity>() where TEntity : BaseEntity;

        Task<int> SaveChangesAsync();
    }
}
