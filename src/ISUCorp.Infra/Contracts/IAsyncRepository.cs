using ISUCorp.Core.Kernel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ISUCorp.Infra.Contracts
{
    public interface IAsyncRepository<T> where T : BaseEntity
    {
        Task<T> FindByIdAsync(int id);
        Task<T> FindByIdExtendedAsync(int id, params Expression<Func<T, object>>[] includes);

        Task<List<T>> ListAsync();
        Task<List<T>> ListAsync(int pageIndex, int pageSize);
        Task<List<T>> ListAsync(int pageIndex, int pageSize, ISpecification<T> spec);
        Task<List<T>> ListAsync(ISpecification<T> spec);
        Task<List<T>> ListAsync(params Expression<Func<T, object>>[] includes);

        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entites);

        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

        Task<T> SingleAsync(ISpecification<T> spec);
        Task<T> SingleOrDefaultAsync(ISpecification<T> spec);
        Task<T> FirstAsync(ISpecification<T> spec);
        Task<T> FirstOrDefaultAsync(ISpecification<T> spec);
        Task<T> LastAsync(ISpecification<T> spec);
        Task<T> LastOrDefaultAsync(ISpecification<T> spec);
        Task<int> CountAsync();
        Task<int> CountAsync(ISpecification<T> spec);
        Task<bool> AnyAsync();
        Task<bool> AnyAsync(ISpecification<T> spec);
    }
}
