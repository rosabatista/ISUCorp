using ISUCorp.Core.Domain;
using ISUCorp.Infra.Contexts;
using ISUCorp.Infra.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISUCorp.Infra.Repositories
{
    public class ContactRepository : EfRepository<Contact, CoreDbContext>, IContactRepository
    {
        public ContactRepository(CoreDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Contact>> SearchByName(string term)
        {
            return await _dbContext.Contacts
                .FromSqlRaw<Contact>("spSearchContactsByName {0}", term)
                .ToListAsync();
        }
    }
}
