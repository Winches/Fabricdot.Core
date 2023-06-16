using Fabricdot.Infrastructure.Uow.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.Tests.Uow;

public class UnitOfWork_CommitChangesAsync_Tests : IntegrationTestBase<InfrastructureTestModule>
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public UnitOfWork_CommitChangesAsync_Tests()
    {
        _unitOfWorkManager = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
    }

    [Fact]
    public async Task CommitChangesAsync_InvokeOnce_Inactive()
    {
        using var uow = _unitOfWorkManager.Begin();
        uow.IsActive.Should().BeTrue();

        await uow.CommitChangesAsync();

        uow.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task CommitChangesAsync_InvokeTwice_ThrowException()
    {
        using var uow = _unitOfWorkManager.Begin();
        await uow.CommitChangesAsync();

        await Awaiting(() => uow.CommitChangesAsync())
                           .Should()
                           .ThrowAsync<InvalidOperationException>();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task CommitChangesAsync_GivenNestedUow_CorrectlyState(bool requireNew)
    {
        using (var uow = _unitOfWorkManager.Begin())
        {
            using (var nestUow = _unitOfWorkManager.Begin(requireNew: requireNew))
            {
                nestUow.IsActive.Should().BeTrue();
                await nestUow.CommitChangesAsync();
                //child unit of work commit nothing
                nestUow.IsActive.Should().Be(!requireNew);
            }

            uow.IsActive.Should().BeTrue();
            await uow.CommitChangesAsync();
            uow.IsActive.Should().BeFalse();
        }
    }

    [Fact]
    public async Task CommitChangesAsync_GivenDiposedUow_ThrowException()
    {
        var uow = _unitOfWorkManager.Begin();
        uow.Dispose();

        await Awaiting(() => uow.CommitChangesAsync())
                           .Should()
                           .ThrowAsync<InvalidOperationException>();
    }
}
