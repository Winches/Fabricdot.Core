using Fabricdot.Core.Boot;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.WebApi.Tests;

public class Startup
{
    public virtual void ConfigureServices(IServiceCollection services)
    {
        services.AddBootstrapper<StartupModule>();
    }

    public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.Bootstrap();
    }
}