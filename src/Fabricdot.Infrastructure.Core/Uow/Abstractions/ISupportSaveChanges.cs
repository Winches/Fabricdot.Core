using System.Threading;
using System.Threading.Tasks;

namespace Fabricdot.Infrastructure.Core.Uow.Abstractions
{
    public interface ISupportSaveChanges
    {
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}