using System.Linq;
using Fabricdot.Core.Modularity;
using Fabricdot.Domain;
using Fabricdot.Infrastructure.Data.Filters;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure
{
    [Requires(typeof(FabricdotDomainModule))]
    [Exports]
    public class FabricdotInfrastructureModule : ModuleBase
    {
        public override void ConfigureServices(ConfigureServiceContext context)
        {
            var services = context.Services;

            //repository filter
            services.AddSingleton(typeof(IDataFilter<>), typeof(DataFilter<>));

            //var assemblies = new TypeFinder().GetAssemblies().ToArray();
            // TODO: Refactor it
            var assemblies = services.GetRequiredSingletonInstance<IModuleCollection>().Select(v => v.Assembly).ToArray();
            services.AddMediatR(assemblies) //mediator
                .AddAutoMapper(assemblies); //mapper
        }
    }
}