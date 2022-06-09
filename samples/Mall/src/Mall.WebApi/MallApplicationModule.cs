using System;
using System.Threading.Tasks;
using Fabricdot.Core.Boot;
using Fabricdot.Core.Modularity;
using Fabricdot.Domain.SharedKernel;
using Fabricdot.WebApi;
using Fabricdot.WebApi.Configuration;
using Mall.Domain;
using Mall.Infrastructure;
using Mall.WebApi.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

        #region endpoint

        services.AddControllers(opts => opts.AddActionFilters())
            .ConfigureApiBehaviorOptions(opts =>
            {
                opts.SuppressModelStateInvalidFilter = true;
            });

        #endregion endpoint

        #region api-doc

        //swagger
        services.AddSwagger();

        #endregion api-doc

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