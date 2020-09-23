using ISUCorp.Core.Domain;
using ISUCorp.Infra.Contexts;
using ISUCorp.Infra.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISUCorp.Infra.Repositories
{
    public class PlaceRepository : EfRepository<Place, CoreDbContext>, IPlaceRepository
    {
        public PlaceRepository(CoreDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Place>> SearchByName(string term)
        {
            return await _dbContext.Places
                .FromSqlRaw<Place>("spSearchPlacesByName {0}", term)
                .ToListAsync();
        }
    }
}
