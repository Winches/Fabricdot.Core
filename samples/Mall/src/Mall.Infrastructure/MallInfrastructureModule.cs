using Fabricdot.Infrastructure.Core.DependencyInjection;
using Mall.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mall.Infrastructure
{
    public class MallInfrastructureModule : IModule
    {
        private readonly IConfiguration _configuration;

        public MallInfrastructureModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(IServiceCollection services)
        {
            services.AddTransient<DbMigrator>();
            services.AddTransient<DataBuilder>();
        }
    }
}