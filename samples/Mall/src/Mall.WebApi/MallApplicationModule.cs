using Fabricdot.Core.Boot;
using Fabricdot.Core.Modularity;
using Fabricdot.Domain.SharedKernel;
using Fabricdot.WebApi;
using Mall.Domain;
using Mall.Infrastructure;
using Mall.WebApi.Configuration;
using Microsoft.AspNetCore.StaticFiles;

namespace Mall.WebApi;

[Requires(typeof(MallDomainModule))]
[Requires(typeof(MallInfrastructureModule))]
[Requires(typeof(FabricdotWebApiModule))]
[Exports]
public class MallApplicationModule : ModuleBase
{
    public override void ConfigureServices(ConfigureServiceContext context)
    {
        var services = context.Services;

        services.AddControllers();

        services.AddSwagger();

        SystemClock.Configure(DateTimeKind.Utc);
        services.AddSingleton<IContentTypeProvider, FileExtensionContentTypeProvider>();
    }

    public override Task OnStartingAsync(ApplicationStartingContext context)
    {
        var app = context.ServiceProvider.GetApplicationBuilder();
        var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UserSwagger();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        return Task.CompletedTask;
    }
}