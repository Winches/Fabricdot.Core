using Fabricdot.Core.Boot;
using Fabricdot.Core.Modularity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.WebApi.Tests.Boot;

public class ModularityStartup<TModule> where TModule : class, IModule
{
    public virtual void ConfigureServices(IServiceCollection services)
    {
        services.AddBootstrapper<TModule>();
    }

    public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.Bootstrap();
    }
}