using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.DependencyInjection
{
    public static class ModuleExtensions
    {
        public static void RegisterModule(this IServiceCollection services, IModule module)
        {
            module.Configure(services);
        }

        public static void RegisterModules(this IServiceCollection services, params IModule[] modules)
        {
            modules.ForEach(services.RegisterModule);
        }
    }
}