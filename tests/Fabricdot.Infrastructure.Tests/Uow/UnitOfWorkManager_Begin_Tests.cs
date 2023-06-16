using Fabricdot.Infrastructure.Uow.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.Tests.Uow;

public class UnitOfWorkManager_Begin_Tests : IntegrationTestBase<InfrastructureTestModule>
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public UnitOfWorkManager_Begin_Tests()
    {
        _unitOfWorkManager = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
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
        nestedUow.Should().NotBe(rootUow);
        nestedUow.Id.Should().Be(rootUow.Id);

        await nestedUow.CommitChangesAsync();

        nestedUow.IsActive.Should().BeTrue();
        rootUow.IsActive.Should().BeTrue();

        await rootUow.CommitChangesAsync();
    }

    [Fact]
    public async Task Begin_RequireNewNestedUow_ReturnNewUow()
    {
        using var rootUow = _unitOfWorkManager.Begin();
        using var nestedUow = _unitOfWorkManager.Begin(requireNew: true);

        AssertNewUnitOfWork(nestedUow);
        nestedUow.Should().NotBe(rootUow);
        nestedUow.Id.Should().NotBe(rootUow.Id);

        await nestedUow.CommitChangesAsync();

        nestedUow.IsActive.Should().BeFalse();
        rootUow.IsActive.Should().BeTrue();

        await rootUow.CommitChangesAsync();
    }

    private static void AssertNewUnitOfWork(IUnitOfWork uow)
    {
        uow.Id.Should().NotBe(default(Guid));
        uow.IsActive.Should().BeTrue();
    }
}
