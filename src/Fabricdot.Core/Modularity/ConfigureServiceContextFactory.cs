using Ardalis.GuardClauses;
using Fabricdot.Core.Boot;
using Fabricdot.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Fabricdot.Core.Modularity
{
    internal class ConfigureServiceContextFactory : IConfigureServiceContextFactory
    {
        private readonly IBootstrapperBuilder _bootstrapperBuilder;

        public ConfigureServiceContextFactory(IBootstrapperBuilder bootstrapperBuilder)
        {
            _bootstrapperBuilder = Guard.Against.Null(bootstrapperBuilder, nameof(bootstrapperBuilder));
        }

        public ConfigureServiceContext Create()
        {
            var services = _bootstrapperBuilder.Services;
            var hostBuilderContext = services.GetSingletonInstance<HostBuilderContext>();

            // Use host configuration first.
            var configuration = hostBuilderContext?.Configuration
                                ?? services.GetSingletonInstance<IConfiguration>();
            // TODO:Use options
            configuration ??= ConfigurationFactory.Build();

            return new ConfigureServiceContext(services, configuration);
        }
    }
}