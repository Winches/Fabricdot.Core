using Fabricdot.Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.Tests.Uow;

public class UnitOfWorkInterceptor_TargetType_Tests : IntegrationTestBase<InfrastructureTestModule>
{
    private readonly IFakeServiceWithUowScope _testService;

    public UnitOfWorkInterceptor_TargetType_Tests()
    {
        _testService = ServiceProvider.GetRequiredService<IFakeServiceWithUowScope>();
    }

    [Fact]
    public void UnitOfWorkInterceptor_BeginUowAutomatically()
    {
        _testService.UseTransactionalUow(uow =>
        {
            uow.Should().NotBeNull();
            uow.IsActive.Should().BeTrue();
        });
    }

    protected override void ConfigureServices(IServiceCollection serviceCollection)
    {
        UseServiceProviderFactory<FabricdotServiceProviderFactory>();
    }
}
