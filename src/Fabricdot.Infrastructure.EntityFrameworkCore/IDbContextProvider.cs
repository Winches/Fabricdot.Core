using System.Threading;
using System.Threading.Tasks;
using Fabricdot.Infrastructure.Uow.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Fabricdot.Infrastructure.EntityFrameworkCore;

public interface IDbContextProvider<TDbContext> : IUnitOfWorkManagerAccessor where TDbContext : DbContext
{
    Task<TDbContext> GetDbContextAsync(CancellationToken cancellationToken = default);
}