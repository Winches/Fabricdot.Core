using System.Threading;
using System.Threading.Tasks;

namespace Fabricdot.Infrastructure.EntityFrameworkCore
{
    public class EfUnitOfWork<TDbContext> : IUnitOfWork<TDbContext> where TDbContext : DbContextBase
    {
        private readonly TDbContext _dbContext;

        public EfUnitOfWork(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc />
        public async Task CommitChangesAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}