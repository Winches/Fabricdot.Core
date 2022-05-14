using Fabricdot.Core.DependencyInjection;
using Fabricdot.Core.Modularity;
using Fabricdot.Domain.DependencyInjection;

namespace Fabricdot.Domain
{
    [Exports]
    public class FabricdotDomainModule : ModuleBase, IPreConfigureService
    {
        public void PreConfigureServices(ConfigureServiceContext context)
        {
            context.Services.AddDependencyRegistrar<DomainDependencyRegistrar>();
        }
    }
}