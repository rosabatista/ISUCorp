using ISUCorp.Infra.Contexts;
using ISUCorp.Infra.Contracts;
using System.Threading.Tasks;

namespace ISUCorp.Infra.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly CoreDbContext _context;

        public UnitOfWork(CoreDbContext context)
        {
            _context = context;
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
