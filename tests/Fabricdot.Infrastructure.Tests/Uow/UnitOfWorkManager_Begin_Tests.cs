using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Fabricdot.Infrastructure.Uow.Abstractions;
using Fabricdot.Test.Shared;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.Infrastructure.Tests.Uow;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class UnitOfWorkManager_Begin_Tests : IntegrationTestBase<InfrastructureTestModule>
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public UnitOfWorkManager_Begin_Tests()
    {
        var provider = ServiceScope.ServiceProvider;
        _unitOfWorkManager = provider.GetRequiredService<IUnitOfWorkManager>();
    }

    [Fact]
    public async Task Begin_WithoutOuterUow_ReturnNewUow()
    {
        using var uow = _unitOfWorkManager.Begin();
        AssertNewUnitOfWork(uow);
        await uow.CommitChangesAsync();
    }

    [Fact]
    public async Task Begin_NotRequireNewNestedUow_ReturnChildUow()
    {
        using var rootUow = _unitOfWorkManager.Begin();
        using var nestedUow = _unitOfWorkManager.Begin(requireNew: false);
        AssertNewUnitOfWork(nestedUow);
        Assert.NotSame(rootUow, nestedUow);
        Assert.Equal(rootUow.Id, nestedUow.Id);
        await nestedUow.CommitChangesAsync();

        Assert.True(nestedUow.IsActive);
        Assert.True(rootUow.IsActive);
        await rootUow.CommitChangesAsync();
    }

    [Fact]
    public async Task Begin_RequireNewNestedUow_ReturnNewUow()
    {
        using var rootUow = _unitOfWorkManager.Begin();
        using var nestedUow = _unitOfWorkManager.Begin(requireNew: true);
        AssertNewUnitOfWork(nestedUow);
        Assert.NotSame(rootUow, nestedUow);
        Assert.NotEqual(rootUow.Id, nestedUow.Id);
        await nestedUow.CommitChangesAsync();

        Assert.False(nestedUow.IsActive);
        Assert.True(rootUow.IsActive);
        await rootUow.CommitChangesAsync();
    }

    private static void AssertNewUnitOfWork(IUnitOfWork uow)
    {
        Assert.NotNull(uow);
        Assert.NotEqual(default, uow.Id);
        Assert.True(uow.IsActive);
    }
}