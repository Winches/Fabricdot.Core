using Fabricdot.Core.Modularity;
using Fabricdot.Domain;
using Fabricdot.Infrastructure.Data.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure;

[Requires(typeof(FabricdotDomainModule))]
[Exports]
public class FabricdotInfrastructureModule : ModuleBase
{
    public override void ConfigureServices(ConfigureServiceContext context)
    {
        var services = context.Services;
        var assemblies = context.GetAssemblies();

        // repository filter
        services.AddSingleton(typeof(IDataFilter<>), typeof(DataFilter<>));
        // mediator
        services.AddMediatR(opts => opts.RegisterServicesFromAssemblies(assemblies));
        // mapper
        services.AddAutoMapper(assemblies);
    }
}
