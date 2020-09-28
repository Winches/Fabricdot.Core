using System.Threading.Tasks;

namespace Fabricdot.Infrastructure.Core.Data
{
    public interface IUnitOfWork
    {
        Task<int> CommitChangesAsync();
    }
}