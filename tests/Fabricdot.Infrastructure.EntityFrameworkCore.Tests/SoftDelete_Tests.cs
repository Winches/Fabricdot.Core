using Fabricdot.Domain.Auditing;
using Fabricdot.Infrastructure.Data.Filters;
using Fabricdot.Infrastructure.Domain.Services;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Fabricdot.Infrastructure.Uow.Abstractions;
using Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests;

public class SoftDelete_Tests : EntityFrameworkCoreTestsBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IDataFilter _dataFilter;

    public SoftDelete_Tests()
    {
        _orderRepository = ServiceProvider.GetRequiredService<IOrderRepository>();
        _dataFilter = ServiceProvider.GetRequiredService<IDataFilter>();
    }

    [Fact]
    public async Task DeleteAsync_GivenISoftDelete_SoftDelete()
    {
        var orderId = Guid.Empty;
        await UseUowAsync(async () =>
        {
            var orders = await _orderRepository.ListAsync();
            var order = orders[0];
            orderId = order.Id;
            await _orderRepository.DeleteAsync(order);
        });

        var order1 = await _orderRepository.GetByIdAsync(orderId);

        order1.Should().BeNull();

        using var scope = _dataFilter.Disable<ISoftDelete>();
        var order2 = await _orderRepository.GetByIdAsync(orderId);

        order2.Should().NotBeNull();
        order2!.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task HardDeleteAsync_GivenISoftDelete_HardDelete()
    {
        var orderId = FakeDataBuilder.OrderId;
        await UseUowAsync(async () =>
        {
            var order = await _orderRepository.GetByIdAsync(FakeDataBuilder.OrderId);
            await _orderRepository.HardDeleteAsync(order!);
        });

        using var scope = _dataFilter.Disable<ISoftDelete>();
        var order = await _orderRepository.GetByIdAsync(orderId);

        order.Should().BeNull();
    }

    private async Task UseUowAsync(Func<Task> func)
    {
        var uowMgr = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        using var uow = uowMgr.Begin(requireNew: true);
        await func();
        await uow.CommitChangesAsync();
    }
}
