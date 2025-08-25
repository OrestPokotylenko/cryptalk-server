using Microsoft.EntityFrameworkCore;

namespace Cryptalk.Infrastructure.Persistence.Repositories
{
    public class EfRepository<T> where T : class
    {
        private readonly CryptalkDbContext _context;
        private readonly DbSet<T> _dbSet;

        public EfRepository(CryptalkDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity, CancellationToken ct)
        {
            await _dbSet.AddAsync(entity, ct);
        }

        public async Task<T?> GetByIdAsync(object id, CancellationToken ct)
        {
            return await _dbSet.FindAsync([id], ct);
        }

        public void Delete(T entity) {
            _dbSet.Remove(entity);
        }

        public async Task SaveChangesAsync(CancellationToken ct)
        {
            await _context.SaveChangesAsync(ct);
        }
    }
}