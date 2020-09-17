using ISUCorp.Core.Kernel;
using ISUCorp.Infra.Contexts;
using ISUCorp.Infra.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISUCorp.Infra.Repositories
{
    /// <summary>
    /// Represents a unit of work with one or more repositories.
    /// </summary>
    /// <typeparam name="TContext">The context where the unit of work will operate.</typeparam>
    public class UnitOfWork<TContext> : IUnitOfWork<TContext>
        where TContext : CoreDbContext, IDisposable
    {
        /// <summary>
        /// Initializes an instance of <see cref="UnitOfWork{TContext}"/>.
        /// </summary>
        /// <param name="context">The context where the unit of work will operate.</param>
        public UnitOfWork(TContext context)
        {
            DbContext = context ?? throw new ArgumentNullException("No context provided.");
            _repositories = new Dictionary<Type, object>();
        }

        /// <summary>
        /// Active repositories to work with.
        /// </summary>
        protected Dictionary<Type, object> _repositories;

        /// <summary>
        /// The context to work with.
        /// </summary>
        public TContext DbContext { get; private set; }

        /// <summary>
        /// Gets a specific repository of an entity.
        /// </summary>
        /// <typeparam name="TEntity">Entity.</typeparam>
        /// <returns>Repository of an entity.</returns>
        public IAsyncRepository<TEntity> GetRepositoryAsync<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity);

            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new EfRepository<TEntity, TContext>(DbContext);
            }

            return (IAsyncRepository<TEntity>) _repositories[type];
        }

        /// <summary>
        /// Saves all pendant changes in the context.
        /// </summary>
        /// <returns>A task that represents the asynchronous save operation. 
        /// The task result contains the number of state entries written to the database.</returns>
        public async Task<int> SaveChangesAsync()
        {
            return await DbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            DbContext?.Dispose();
        }
    }
}
