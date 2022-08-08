using Fabricdot.AspNetCore.Testing.Middleware;
using Fabricdot.Core.Boot;
using Fabricdot.Core.Modularity;
using Fabricdot.Infrastructure;
using Fabricdot.WebApi.Tracing;
using Fabricdot.WebApi.Uow;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.WebApi.Tests;

[Requires(typeof(FabricdotInfrastructureModule))]
[Requires(typeof(FabricdotWebApiModule))]
[Exports]
public class WebApiTestModule : ModuleBase
{
    public override void ConfigureServices(ConfigureServiceContext context)
    {
        var services = context.Services;

        services.AddControllers();
    }

    public override Task OnStartingAsync(ApplicationStartingContext context)
    {
        var app = context.ServiceProvider.GetApplicationBuilder();

        app.UseMiddleware<RequestBodyKeeperMiddleware>();
        app.UseCorrelationId();
        app.UseRouting();
        app.UseUnitOfWork();
        app.UseMiddleware<ActionMiddleware>();
        app.UseEndpoints(endpoints => endpoints.MapControllers());

        return Task.CompletedTask;
    }
}