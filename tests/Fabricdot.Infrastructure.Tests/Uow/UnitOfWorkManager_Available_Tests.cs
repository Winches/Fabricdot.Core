using Fabricdot.Infrastructure.Uow.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.Tests.Uow;

public class UnitOfWorkManager_Available_Tests : IntegrationTestBase<InfrastructureTestModule>
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public UnitOfWorkManager_Available_Tests()
    {
        _unitOfWorkManager = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
    }

    [Fact]
    public void Available_NoExistedUow_ReturnNull()
    {
        _unitOfWorkManager.Available.Should().BeNull();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Available_BeginNestedUow_ReturnCorrectUow(bool requireNew)
    {
        using var rootUow = _unitOfWorkManager.Begin();
        _unitOfWorkManager.Available.Should().BeSameAs(rootUow);

        using (var nestedUow = _unitOfWorkManager.Begin(requireNew: requireNew))
        {
            //ignore child unit of work
            _unitOfWorkManager.Available.Should().BeSameAs(requireNew ? nestedUow : rootUow);

            await nestedUow.CommitChangesAsync();
        }

        _unitOfWorkManager.Available.Should().BeSameAs(rootUow);

        await rootUow.CommitChangesAsync();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Available_CommitChanges_ReturnOuterUow(bool requireNew)
    {
        using var rootUow = _unitOfWorkManager.Begin();
        using (var nestedUow = _unitOfWorkManager.Begin(requireNew: requireNew))
        {
            await nestedUow.CommitChangesAsync();

            _unitOfWorkManager.Available.Should().BeSameAs(rootUow);
        }

        await rootUow.CommitChangesAsync();

        _unitOfWorkManager.Available.Should().BeNull();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Available_DisposeUow_ReturnOuterUow(bool requireNew)
    {
        using (var rootUow = _unitOfWorkManager.Begin())
        {
            using (var nestedUow = _unitOfWorkManager.Begin(requireNew: requireNew))
                await nestedUow.CommitChangesAsync();

            _unitOfWorkManager.Available.Should().BeSameAs(rootUow);

            await rootUow.CommitChangesAsync();
        }
        _unitOfWorkManager.Available.Should().BeNull();
    }
}
