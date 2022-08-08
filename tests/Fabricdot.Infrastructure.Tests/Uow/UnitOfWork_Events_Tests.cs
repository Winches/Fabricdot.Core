using Fabricdot.Infrastructure.Uow.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Fabricdot.Infrastructure.Tests.Uow;

public class UnitOfWork_Events_Tests : IntegrationTestBase<InfrastructureTestModule>
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public UnitOfWork_Events_Tests()
    {
        _unitOfWorkManager = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
    }

    [Fact]
    public void Dispose_SubscribeDisposed_TriggerEventHandler()
    {
        var mock = Mock<Action>();
        using (var uow = _unitOfWorkManager.Begin())
        {
            uow.Disposed += (_, __) => mock.Object();
            mock.Verify(v => v(), Times.Never);
        }
        mock.Verify(v => v(), Times.Once);
    }

    [Fact]
    public void Dispose_SubscribeChildUowDisposed_TriggerEventHandler()
    {
        var mock = Mock<Action>();
        using (var uow1 = _unitOfWorkManager.Begin())
        {
            using (var uow2 = _unitOfWorkManager.Begin())
            {
                uow1.Disposed += (_, __) => mock.Object();
            }
            mock.Verify(v => v(), Times.Never);
        }
        mock.Verify(v => v(), Times.Once);
    }
}