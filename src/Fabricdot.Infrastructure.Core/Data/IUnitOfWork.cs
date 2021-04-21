using System.Threading;
using System.Threading.Tasks;

namespace Fabricdot.Infrastructure.Core.Data
{
    public interface IUnitOfWork
    {
        Task CommitChangesAsync(CancellationToken cancellationToken = default);
    }
}