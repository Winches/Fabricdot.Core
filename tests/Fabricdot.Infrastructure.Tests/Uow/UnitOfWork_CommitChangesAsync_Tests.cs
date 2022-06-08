using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Fabricdot.Infrastructure.Uow.Abstractions;
using Fabricdot.Test.Shared;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.Infrastructure.Tests.Uow;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class UnitOfWork_CommitChangesAsync_Tests : IntegrationTestBase<InfrastructureTestModule>
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public UnitOfWork_CommitChangesAsync_Tests()
    {
        var provider = ServiceScope.ServiceProvider;
        _unitOfWorkManager = provider.GetRequiredService<IUnitOfWorkManager>();
    }

    [Fact]
    public async Task CommitChangesAsync_InvokeOnce_Inactive()
    {
        using (var uow = _unitOfWorkManager.Begin())
        {
            Assert.True(uow.IsActive);
            await uow.CommitChangesAsync();
            Assert.False(uow.IsActive);
        }
    }

    [Fact]
    public async Task CommitChangesAsync_InvokeTwice_ThrowException()
    {
        using (var uow = _unitOfWorkManager.Begin())
        {
            await uow.CommitChangesAsync();
            async Task testCode() => await uow.CommitChangesAsync();
            await Assert.ThrowsAsync<InvalidOperationException>(testCode);
        }
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
                Assert.True(nestUow.IsActive);
                await nestUow.CommitChangesAsync();
                if (requireNew)
                    Assert.False(nestUow.IsActive);
                else
                    Assert.True(nestUow.IsActive);//child unit of work commit nothing
            }

            Assert.True(uow.IsActive);
            await uow.CommitChangesAsync();
            Assert.False(uow.IsActive);
        }
    }

    [Fact]
    public async Task CommitChangesAsync_GivenDiposedUow_ThrowException()
    {
        var uow = _unitOfWorkManager.Begin();
        uow.Dispose();
        async Task testCode() => await uow.CommitChangesAsync();
        await Assert.ThrowsAsync<InvalidOperationException>(testCode);
    }
}