using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Cryptalk.Infrastructure.Persistance
{
    public class CryptalkDbContextFactory : IDesignTimeDbContextFactory<CryptalkDbContext>
    {
        public CryptalkDbContext CreateDbContext(string[] args)
        {
            // 1) шукаємо connection string у env var
            var cs = Environment.GetEnvironmentVariable("ConnectionStrings__Default");

            var optionsBuilder = new DbContextOptionsBuilder<CryptalkDbContext>();
            optionsBuilder.UseNpgsql(cs);

            return new CryptalkDbContext(optionsBuilder.Options);
        }
    }
}