using Fabricdot.Infrastructure.DependencyInjection;
using Fabricdot.Infrastructure.Uow;
using Fabricdot.Infrastructure.Uow.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.Tests.Uow;

public class UnitOfWorkInterceptorTests : IntegrationTestBase<InfrastructureTestModule>
{
    private readonly FakeServiceWithUnitOfWorkInterceptor _testService;
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly IUnitOfWorkTransactionBehaviourProvider _transactionBehaviourProvider;

    public UnitOfWorkInterceptorTests()
    {
        _testService = ServiceProvider.GetRequiredService<FakeServiceWithUnitOfWorkInterceptor>();
        _unitOfWorkManager = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        _transactionBehaviourProvider = ServiceProvider.GetRequiredService<IUnitOfWorkTransactionBehaviourProvider>();
    }

    [Fact]
    public void UnitOfWorkInterceptor_SpecificNothing_BeginUowAutomatically()
    {
        _testService.UseAutomaticTransactionalUow(uow =>
        {
            const string actionName = nameof(FakeServiceWithUnitOfWorkInterceptor.UseAutomaticTransactionalUow);
            var behaviour = _transactionBehaviourProvider.GetBehaviour(actionName);

            uow.Options.IsTransactional.Should().Be(behaviour);
            uow.IsActive.Should().BeTrue();
        });
        _unitOfWorkManager.Available.Should().BeNull();
    }

    [Fact]
    public void UnitOfWorkInterceptor_SpecificTransactional_BeginTransactionalUow()
    {
        _testService.UseTransactionalUow(uow =>
        {
            uow.Options.IsTransactional.Should().BeTrue();
            uow.IsActive.Should().BeTrue();
        });
        _unitOfWorkManager.Available.Should().BeNull();
    }

    [Fact]
    public void UnitOfWorkInterceptor_SpecificNotTransactional_BeginNoTransactionalUow()
    {
        _testService.UseNotTransactionalUow(uow =>
        {
            uow.Options.IsTransactional.Should().BeFalse();
            uow.IsActive.Should().BeTrue();
        });
        _unitOfWorkManager.Available.Should().BeNull();
    }

    [Fact]
    public void UnitOfWorkInterceptor_WithReservedUow_BeginReservedUow()
    {
        using var uow = _unitOfWorkManager.Reserve(UnitOfWorkManager.RESERVATION_NAME);

        _testService.UseAutomaticTransactionalUow(uow => uow.IsActive.Should().BeTrue());
        _unitOfWorkManager.Available.Should().Be(uow);
        uow.IsActive.Should().BeTrue();
    }

    protected override void ConfigureServices(IServiceCollection serviceCollection)
    {
        UseServiceProviderFactory<FabricdotServiceProviderFactory>();
    }
}