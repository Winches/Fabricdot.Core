using System.Threading.Tasks;

namespace Fabricdot.Core.Boot;

public interface IApplicationStarting
{
    Task OnStartingAsync(ApplicationStartingContext context);
}