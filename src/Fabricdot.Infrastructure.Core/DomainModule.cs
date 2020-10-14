using System.Reflection;
using Fabricdot.Common.Core.Enumerable;
using Fabricdot.Infrastructure.Core.DependencyInjection;
using Fabricdot.Infrastructure.Core.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.Core
{
    public sealed class DomainModule : IModule
    {
        private readonly Assembly _assembly;

        public DomainModule(Assembly assembly)
        {
            _assembly = assembly;
        }

        /// <inheritdoc />
        public void Configure(IServiceCollection services)
        {
            DomainProvider.GetServices(_assembly).ForEach(v => services.AddScoped(v.Contract, v.Implementation));
            DomainProvider.GetRepository(_assembly).ForEach(v => services.AddScoped(v.Contract, v.Implementation));
        }
    }
}