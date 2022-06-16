using Fabricdot.AspNetCore.Testing.Middleware;
using Fabricdot.Core.Boot;
using Fabricdot.Core.Modularity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.AspNetCore.Testing;

public class TestStartup<TModule> where TModule : class, IModule
{
    public virtual void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<RequestBodyKeeperMiddleware>();
        services.AddBootstrapper<TModule>();
        services.AddControllers().AddApplicationPart(typeof(TModule).Assembly);
        services.AddTransient<RequestBodyKeeperMiddleware>();
    }

    public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.Bootstrap();
    }
}