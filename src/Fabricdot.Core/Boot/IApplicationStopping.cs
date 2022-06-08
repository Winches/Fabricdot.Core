using System.Threading.Tasks;

namespace Fabricdot.Core.Boot;

public interface IApplicationStopping
{
    Task OnStoppingAsync(ApplicationStoppingContext context);
}