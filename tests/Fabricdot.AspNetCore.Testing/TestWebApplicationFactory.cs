using Fabricdot.Core.Modularity;
using Fabricdot.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace Fabricdot.AspNetCore.Testing;

public class TestWebApplicationFactory<TModule> : WebApplicationFactory<TestStartup<TModule>> where TModule : class, IModule
{
    // Override methods here as needed
    protected override IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
                   .ConfigureWebHostDefaults(builder => builder.UseStartup<TestStartup<TModule>>())
                   .UseServiceProviderFactory(new FabricdotServiceProviderFactory());
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        // https://github.com/dotnet/aspnetcore/issues/17707#issuecomment-609061917
        builder.UseContentRoot(Directory.GetCurrentDirectory());
        return base.CreateHost(builder);
    }
}