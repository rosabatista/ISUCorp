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
    public class EfRepository<T, TC> : IAsyncRepository<T>
        where T : BaseEntity
        where TC : DbContext
    {
        protected readonly TC _dbContext;

        public EfRepository(TC dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entites)
        {
            await _dbContext.Set<T>().AddRangeAsync(entites);
        }

        public async Task<bool> AnyAsync()
        {
            return await _dbContext.Set<T>().AnyAsync();
        }

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

        public async Task<int> CountAsync()
        {
            return await _dbContext.Set<T>().CountAsync();
        }

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
