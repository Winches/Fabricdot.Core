using Fabricdot.Infrastructure.Core.Data;

namespace Fabricdot.Infrastructure.EntityFrameworkCore
{
    public interface IUnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContextBase
    {
    }
}