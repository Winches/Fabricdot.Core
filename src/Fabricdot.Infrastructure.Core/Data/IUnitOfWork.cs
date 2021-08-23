using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fabricdot.Infrastructure.Core.Data
{
    [Obsolete("This will be removed in future.")]
    public interface IUnitOfWork
    {
        Task CommitChangesAsync(CancellationToken cancellationToken = default);
    }
}