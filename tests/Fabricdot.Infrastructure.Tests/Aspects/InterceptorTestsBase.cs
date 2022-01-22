using AspectCore.Extensions.DependencyInjection;
using Fabricdot.Infrastructure.Tests.Aspects.Interceptors;
using Fabricdot.Test.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.Tests.Aspects
{
    public abstract class InterceptorTestsBase : IntegrationTestBase
    {
        protected readonly ICalculator Calculator;
        protected readonly ICalculator DerivedCalculator;

        /// <inheritdoc />
        protected InterceptorTestsBase()
        {
            Calculator = ServiceProvider.GetRequiredService<ICalculator>();
            DerivedCalculator = ServiceProvider.GetRequiredService<DerivedCalculator>();
        }

        protected static void ResetInterceptorsState()
        {
            IntegerParameterInterceptor.Parameters = default;
            IntegerResultInterceptor.Result = default;
            LoggingInterceptor.IsLogged = default;
        }

        /// <inheritdoc />
        protected override sealed void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ICalculator, Calculator>();
            serviceCollection.AddTransient<DerivedCalculator>();
            serviceCollection.AddTransient<IParameterInterceptor, IntegerParameterInterceptor>();
            serviceCollection.AddTransient<IntegerResultInterceptor>();
            serviceCollection.AddTransient<LoggingInterceptor>();
            serviceCollection.AddTransient<ShouldNotInvokedInterceptor>();
            serviceCollection.AddInterceptors();
            UseServiceProviderFactory<DynamicProxyServiceProviderFactory>();
        }
    }
}