using System.IO;
using Fabricdot.Core.Boot;
using Fabricdot.Core.Modularity;
using Fabricdot.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Fabricdot.PermissionGranting.AspNetCore.Tests;

public class Startup<TModule> where TModule : class, IModule
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

public class WebAppFactory<TModule> : WebApplicationFactory<Startup<TModule>> where TModule : class, IModule
{
    // Override methods here as needed
    protected override IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
                   .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup<TModule>>())
                   .UseServiceProviderFactory(new FabricdotServiceProviderFactory());
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        // https://github.com/dotnet/aspnetcore/issues/17707#issuecomment-609061917
        builder.UseContentRoot(Directory.GetCurrentDirectory());
        return base.CreateHost(builder);
    }
}