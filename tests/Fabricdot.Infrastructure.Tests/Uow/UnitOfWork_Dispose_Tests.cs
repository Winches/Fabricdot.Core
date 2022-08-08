using Fabricdot.Infrastructure.Uow.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.Tests.Uow;

public class UnitOfWork_Dispose_Tests : IntegrationTestBase<InfrastructureTestModule>
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public UnitOfWork_Dispose_Tests()
    {
        _unitOfWorkManager = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
    }

    [Fact]
    public void Dispose_InvokeOnce_Inactive()
    {
        var uow = _unitOfWorkManager.Begin();
        uow.Dispose();

        uow.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Dispose_InvokeTwice_DoNothing()
    {
        var uow = _unitOfWorkManager.Begin();
        uow.Dispose();
        uow.Dispose();

        uow.IsActive.Should().BeFalse();
    }
}