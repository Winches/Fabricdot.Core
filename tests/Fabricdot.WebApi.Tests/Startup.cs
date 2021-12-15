using Fabricdot.Infrastructure;
using Fabricdot.Infrastructure.DependencyInjection;
using Fabricdot.WebApi.Configuration;
using Fabricdot.WebApi.Uow;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.WebApi.Tests
{
    public class Startup
    {
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.RegisterModules(
                new InfrastructureModule(),
                new ApplicationModule());

            services.AddSingleton<ActionMiddlewareProvider>();
            services.AddTransient<ActionMiddleware>();

            services.AddControllers(opts => opts.AddActionFilters())
                .ConfigureApiBehaviorOptions(opts =>
                {
                    opts.SuppressModelStateInvalidFilter = true;
                });
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseUnitOfWork();
            app.UseMiddleware<ActionMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}