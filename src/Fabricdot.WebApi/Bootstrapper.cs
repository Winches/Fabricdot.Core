using System;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Fabricdot.Infrastructure;
using Fabricdot.Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Web;

namespace Fabricdot.WebApi
{
    public static class Bootstrapper
    {
        /// <summary>
        ///     run host asynchronous
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static async Task RunAsync(this IHostBuilder hostBuilder, Func<IHost, Task> func = null)
        {
            var logger = NLogBuilder.ConfigureNLog("logger.config")
                .GetCurrentClassLogger();

            try
            {
                var host = hostBuilder
                    //.ConfigureServices(Register)
                    .ConfigureLogging(builder =>
                    {
                        builder.ClearProviders();
                        builder.AddConsole();
                        builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                    })
                    .UseNLog()
                    .Build();

                LogManager.Configuration.Install(new InstallationContext());
                if (func != null)
                    await func.Invoke(host);

                await host.RunAsync();
                logger.Trace("app starting..");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred when host running.");
            }
            finally
            {
                logger.Trace("app shutting..");
                LogManager.Shutdown();
            }
        }

        public static IServiceCollection AddBasicModules(this IServiceCollection services)
        {
            Guard.Against.Null(services, nameof(services));

            services.RegisterModules(
                new InfrastructureModule(),
                new ApplicationModule());
            return services;
        }
    }
}