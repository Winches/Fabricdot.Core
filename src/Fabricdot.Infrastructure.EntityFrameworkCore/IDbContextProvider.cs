using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Fabricdot.Infrastructure.EntityFrameworkCore
{
    public interface IDbContextProvider<TDbContext> where TDbContext : DbContext
    {
        Task<TDbContext> GetDbContextAsync(CancellationToken cancellationToken = default);
    }
}