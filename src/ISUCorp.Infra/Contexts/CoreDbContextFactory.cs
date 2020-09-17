using ISUCorp.Infra.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ISUCorp.Infra.Contexts
{
    public class CoreDbContextFactory : IDesignTimeDbContextFactory<CoreDbContext>
    {
        private static string DataConnectionString => DbConfiguration.DataConnectionString;

        public CoreDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CoreDbContext>();
            optionsBuilder.UseSqlServer(DataConnectionString);
            return new CoreDbContext(optionsBuilder.Options);
        }
    }
}
