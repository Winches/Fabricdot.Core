using Fabricdot.Common.Core.Reflections;
using Fabricdot.Infrastructure.Core.DependencyInjection;
using Fabricdot.Infrastructure.Core.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.Core
{
    //todo:consider move to infrastructure module
    public sealed class DomainModule : IModule
    {
        /// <inheritdoc />
        public void Configure(IServiceCollection services)
        {
            var assemblies = new TypeFinder().GetAssemblies().ToArray();
            services.AddRepositories(assemblies);
            services.AddDomainServices(assemblies);
        }
    }
}