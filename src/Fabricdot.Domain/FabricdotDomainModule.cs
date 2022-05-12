using Fabricdot.Core.DependencyInjection;
using Fabricdot.Core.Modularity;
using Fabricdot.Domain.DependencyInjection;
using Fabricdot.MultiTenancy.Abstractions;

namespace Fabricdot.Domain
{
    [Requires(typeof(FabricdotMultiTenancyAbstractionModule))]
    [Exports]
    public class FabricdotDomainModule : ModuleBase, IPreConfigureService
    {
        public void PreConfigureServices(ConfigureServiceContext context)
        {
            context.Services.AddDependencyRegistrar<DomainDependencyRegistrar>();
        }
    }
}