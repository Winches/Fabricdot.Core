using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Core.Modularity
{
    public class ConfigureServiceContext
    {
        public IServiceCollection Services { get; }

        public IConfiguration Configuration { get; }

        public ConfigureServiceContext(
            IServiceCollection services,
            IConfiguration configuration)
        {
            Guard.Against.Null(services, nameof(services));
            Guard.Against.Null(configuration, nameof(configuration));

            Services = services;
            Configuration = configuration;
        }
    }
}