using ISUCorp.Core.Kernel;
using ISUCorp.Infra.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ISUCorp.Infra.Repositories
{
    /// <summary>
    /// Represents a repository of an entity.
    /// </summary>
    /// <typeparam name="T">Entity contained in the repository.</typeparam>
    /// <typeparam name="TC">The context where the repository belongs.</typeparam>
    public class EfRepository<T, TC> : IAsyncRepository<T>
        where T : BaseEntity
        where TC : DbContext
    {
        protected readonly TC _dbContext;

        /// <summary>
        /// Initializes an instance of <see cref="EfRepository{T, TC}"/>.
        /// </summary>
        /// <param name="dbContext"></param>
        public EfRepository(TC dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<T> DbSet()
        {
            return _dbContext.Set<T>().AsNoTracking();
        }

        /// <summary>
        /// Adds an entity to the repository.
        /// </summary>
        /// <param name="entity">The entity to be added.</param>
        /// <returns>A task that represents the asynchronous Add operation. The task result contains 
        /// the Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry for the entity. 
        /// The entry provides access to change tracking information and operations for the entity.</returns>
        public async Task AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        /// <summary>
        /// Adds a range of entities to the repository.
        /// </summary>
        /// <param name="entites">Entities to be added to the repository.</param>
        /// <returns></returns>
        public async Task AddRangeAsync(IEnumerable<T> entites)
        {
            await _dbContext.Set<T>().AddRangeAsync(entites);
        }

        /// <summary>
        /// Asynchronously determines whether the repository contains any elements.
        /// </summary>
        /// <returns>The task result contains true if the source sequence 
        /// contains any elements; otherwise, false.</returns>
        public async Task<bool> AnyAsync()
        {
            return await _dbContext.Set<T>().AnyAsync();
        }

        /// <summary>
        /// Asynchronously determines whether the repository contains any elements 
        /// according to a specification.
        /// </summary>
        /// <param name="spec">Specification to considere.</param>
        /// <returns>The task result contains true if the source sequence 
        /// contains any elements; otherwise, false.</returns>
        public async Task<bool> AnyAsync(ISpecification<T> spec)
        {
            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes.Aggregate(_dbContext.Set<T>().AsQueryable(),
              (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings.Aggregate(queryableResultWithIncludes,
              (current, include) => current.Include(include));

            // return the result of the query using the specification's criteria expression
            return await secondaryResult.AnyAsync(spec.Criteria);
        }

        /// <summary>
        /// Asynchronously determines the count of elements in the repository.
        /// </summary>
        /// <returns>The task result contains the count of element in the repository.</returns>
        public async Task<int> CountAsync()
        {
            return await _dbContext.Set<T>().CountAsync();
        }

        /// <summary>
        /// Asynchronously determines the count of elements in the repository 
        /// taking into consideration a specification.
        /// </summary>
        /// <param name="spec">Specification to considere.</param>
        /// <returns>The task result contains the count of element.</returns>
        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes.Aggregate(_dbContext.Set<T>().AsQueryable(),
                (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings.Aggregate(queryableResultWithIncludes,
              (current, include) => current.Include(include));

            // return the result of the query using the specification's criteria expression
            return await secondaryResult
              .Where(spec.Criteria)
              .CountAsync();
        }

        public void Remove(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
        }

        public async Task<T> FindByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> FindByIdExtendedAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            var result = _dbContext.Set<T>().AsQueryable();

            foreach (var include in includes)
            {
                result = result.Include(include);
            }

            return await result.SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<T> FirstAsync(ISpecification<T> spec)
        {
            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes.Aggregate(_dbContext.Set<T>().AsQueryable(),
                (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings.Aggregate(queryableResultWithIncludes,
                (current, include) => current.Include(include));

            // return the result of the query using the specification's criteria expression
            return await secondaryResult.OrderBy(t => t.AddedAt).FirstAsync(spec.Criteria);
        }

        public async Task<T> FirstOrDefaultAsync(ISpecification<T> spec)
        {
            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes.Aggregate(_dbContext.Set<T>().AsQueryable(),
                (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings.Aggregate(queryableResultWithIncludes,
              (current, include) => current.Include(include));

            // return the result of the query using the specification's criteria expression
            return await secondaryResult.OrderBy(t => t.AddedAt).FirstOrDefaultAsync(spec.Criteria);
        }

        public async Task<T> LastAsync(ISpecification<T> spec)
        {
            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes.Aggregate(_dbContext.Set<T>().AsQueryable(),
                (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings.Aggregate(queryableResultWithIncludes,
                (current, include) => current.Include(include));

            // return the result of the query using the specification's criteria expression
            return await secondaryResult.OrderByDescending(t => t.AddedAt).FirstAsync(spec.Criteria);
        }

        public async Task<T> LastOrDefaultAsync(ISpecification<T> spec)
        {
            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes.Aggregate(_dbContext.Set<T>().AsQueryable(),
                (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings.Aggregate(queryableResultWithIncludes,
              (current, include) => current.Include(include));

            // return the result of the query using the specification's criteria expression
            return await secondaryResult.OrderByDescending(t => t.AddedAt).FirstOrDefaultAsync(spec.Criteria);
        }

        public async Task<List<T>> ListAsync()
        {
            return await _dbContext.Set<T>()
              .AsNoTracking()
              .ToListAsync();
        }

        public async Task<List<T>> ListAsync(int pageIndex, int pageSize)
        {
            return await _dbContext.Set<T>()
              .AsNoTracking()
              .Skip((pageIndex - 1) * pageSize)
              .Take(pageSize)
              .ToListAsync();
        }

        public async Task<List<T>> ListAsync(int pageIndex, int pageSize, ISpecification<T> spec)
        {
            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes
              .Aggregate(_dbContext.Set<T>().AsQueryable(), (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings
              .Aggregate(queryableResultWithIncludes, (current, include) => current.Include(include));

            return await secondaryResult
              .AsNoTracking()
              .Where(spec.Criteria)
              .Skip((pageIndex - 1) * pageSize)
              .Take(pageSize)
              .ToListAsync();
        }

        public async Task<List<T>> ListAsync(ISpecification<T> spec)
        {
            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes
              .Aggregate(_dbContext.Set<T>().AsQueryable(), (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings
              .Aggregate(queryableResultWithIncludes, (current, include) => current.Include(include));

            // return the result of the query using the specification's criteria expression
            return await secondaryResult
              .AsNoTracking()
              .Where(spec.Criteria)
              .ToListAsync();
        }

        public async Task<List<T>> ListAsync(params Expression<Func<T, object>>[] includes)
        {
            var result = _dbContext.Set<T>().AsQueryable();

            foreach (var include in includes)
            {
                result = result.Include(include);
            }

            return await result
              .AsNoTracking()
              .ToListAsync();
        }

        public async Task<T> SingleAsync(ISpecification<T> spec)
        {
            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes
              .Aggregate(_dbContext.Set<T>().AsQueryable(),
                (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings
              .Aggregate(queryableResultWithIncludes,
                (current, include) => current.Include(include));

            // return the result of the query using the specification's criteria expression
            return await secondaryResult
              .SingleAsync(spec.Criteria);
        }

        public async Task<T> SingleOrDefaultAsync(ISpecification<T> spec)
        {
            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes
              .Aggregate(_dbContext.Set<T>().AsQueryable(),
                (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings
              .Aggregate(queryableResultWithIncludes,
                (current, include) => current.Include(include));

            // return the result of the query using the specification's criteria expression
            return await secondaryResult
              .SingleOrDefaultAsync(spec.Criteria);
        }

        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().UpdateRange(entities);
        }
    }
}
