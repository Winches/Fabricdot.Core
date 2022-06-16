using Fabricdot.Infrastructure.DependencyInjection;
using Fabricdot.Infrastructure.Tests.Aspects.Interceptors;
using Fabricdot.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.Tests.Aspects;

public abstract class InterceptorTestsBase : IntegrationTestBase<InfrastructureTestModule>
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

    protected override void ConfigureServices(IServiceCollection serviceCollection)
    {
        UseServiceProviderFactory<FabricdotServiceProviderFactory>();
    }
}