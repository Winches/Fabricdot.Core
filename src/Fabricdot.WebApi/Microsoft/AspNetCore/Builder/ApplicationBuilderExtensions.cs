using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Fabricdot.Core.Boot;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        ///     Start an application with the specific <paramref name="applicationBuilder" />.
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <returns></returns>
        public static async Task BootstrapAsync(this IApplicationBuilder applicationBuilder)
        {
            Guard.Against.Null(applicationBuilder, nameof(applicationBuilder));

            var applicationServices = applicationBuilder.ApplicationServices;
            var hostApplicationLifetime = applicationServices.GetRequiredService<IHostApplicationLifetime>();
            var builder = applicationServices.GetRequiredService<IBootstrapperBuilder>();
            applicationServices.GetBuilderProperties().GetOrAdd(BootstrapperBuilderProperties.ApplicationBuilder, _ => applicationBuilder);

            await applicationServices.BootstrapAsync(app =>
            {
                hostApplicationLifetime.ApplicationStopping.Register(() => app.StopAsync().GetAwaiter().GetResult());

                hostApplicationLifetime.ApplicationStopped.Register(() => app.Dispose());
            });
        }

        /// <summary>
        ///     Start an application with the specific <paramref name="applicationBuilder" />.
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <returns></returns>
        public static void Bootstrap(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.BootstrapAsync().GetAwaiter().GetResult();
        }
    }
}