using ISUCorp.Core.Domain;
using ISUCorp.Infra.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace ISUCorp.Infra.Contexts
{
    public class CoreDbContext : DbContext
    {
        public DbSet<Contact> Contacts { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<Place> Places { get; set; }

        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CoreDbContext).Assembly);
        }

        public override int SaveChanges()
        {
            ChangeTracker.ApplyAuditInformation();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ChangeTracker.ApplyAuditInformation();
            return await base.SaveChangesAsync(cancellationToken);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
          CancellationToken cancellationToken = default(CancellationToken))
        {
            ChangeTracker.ApplyAuditInformation();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
