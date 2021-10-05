using Fabricdot.Infrastructure.Core.Tests.Aspects.Interceptors;
using Fabricdot.Test.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.Core.Tests.Aspects
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

        /// <inheritdoc />
        protected sealed override void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ICalculator, Calculator>();
            serviceCollection.AddTransient<DerivedCalculator>();
            serviceCollection.AddTransient<IParameterInterceptor, IntegerParameterInterceptor>();
            serviceCollection.AddTransient<IntegerResultInterceptor>();
            serviceCollection.AddTransient<LoggingInterceptor>();
            serviceCollection.AddTransient<ShouldNotInvokedInterceptor>();

            ServiceProvider = serviceCollection.AddInterceptors().BuildProxiedServiceProvider();
        }

        protected static void ResetInterceptorsState()
        {
            IntegerParameterInterceptor.Parameters = default;
            IntegerResultInterceptor.Result = default;
            LoggingInterceptor.IsLogged = default;
        }
    }
}