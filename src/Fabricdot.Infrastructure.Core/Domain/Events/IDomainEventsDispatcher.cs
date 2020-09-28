using System.Threading.Tasks;

namespace Fabricdot.Infrastructure.Core.Domain.Events
{
    public interface IDomainEventsDispatcher
    {
        Task DispatchEventsAsync();
    }
}