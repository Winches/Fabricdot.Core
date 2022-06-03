using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Fabricdot.Core.Configuration;
using Fabricdot.Core.Modularity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Fabricdot.Core.Boot
{
    public static class BootstrapperExtensions
    {
        /// <summary>
        ///     Create a modularity application with the specific <paramref name="services" />.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="moduleType"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IBootstrapperBuilder AddBootstrapper(
            this IServiceCollection services,
            Type moduleType,
            Action<BootstrapperBuilderOptions>? configureOptions = null)
        {
            Guard.Against.Null(services, nameof(services));
            Guard.Against.Null(moduleType, nameof(moduleType));

            var options = new BootstrapperBuilderOptions(services);
            configureOptions?.Invoke(options);

            return Bootstrapper.CreateBuilder(options)
                               .AddModules(moduleType);
        }

        /// <summary>
        ///     Create a modularity application with the specific <paramref name="services" />.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IBootstrapperBuilder AddBootstrapper<T>(
            this IServiceCollection services,
            Action<BootstrapperBuilderOptions>? configureOptions = null) where T : class, IModule
        {
            return services.AddBootstrapper(typeof(T), configureOptions);
        }

        /// <summary>
        ///     Start an application with the specific <paramref name="serviceProvider" />.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="configureAction"></param>
        public static async Task BootstrapAsync(
            this IServiceProvider serviceProvider,
            Action<IApplication>? configureAction = null)
        {
            Guard.Against.Null(serviceProvider, nameof(serviceProvider));

            var builder = serviceProvider.GetRequiredService<IBootstrapperBuilder>();
            var app = builder.Build(serviceProvider);

            configureAction?.Invoke(app);
            await app.StartAsync();
        }

        /// <summary>
        ///     Get properties of the <see cref="IBootstrapperBuilder" />
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static IDictionary<string, object> GetBuilderProperties(this IServiceProvider serviceProvider)
        {
            Guard.Against.Null(serviceProvider, nameof(serviceProvider));

            return serviceProvider.GetRequiredService<IBootstrapperBuilder>().Properties;
        }

        /// <summary>
        ///     Try add <see cref="IConfiguration" /> to the <paramref name="services" />
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        public static void TryAddConfiguration(
            this IServiceCollection services,
            ConfigurationBuilderOptions? options = null)
        {
            Guard.Against.Null(services, nameof(services));

            if (!services.ContainsService<IConfiguration>())
            {
                var configuration = ConfigurationFactory.Build(options);
                services.Replace(ServiceDescriptor.Singleton<IConfiguration>(configuration));
            }
        }
    }
}