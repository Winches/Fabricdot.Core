using Fabricdot.Domain.Auditing;
using Fabricdot.Infrastructure.Data.Filters;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Fabricdot.Infrastructure.Uow.Abstractions;
using Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;
using Fabricdot.Test.Helpers.Domain.Specifications;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests;

public class SoftDelete_Cascading_Tests : EntityFrameworkCoreTestsBase
{
    private readonly IDataFilter _dataFilter;
    private readonly IOrderRepository _orderRepository;

    public SoftDelete_Cascading_Tests()
    {
        _dataFilter = ServiceProvider.GetRequiredService<IDataFilter>();
        _orderRepository = ServiceProvider.GetRequiredService<IOrderRepository>();
    }

    [Fact]
    public async Task DbContextBase_RemoveEntityOfCascadingCollection_SoftDelete()
    {
        var specification = new OrderWithDetailsSpecification(FakeDataBuilder.OrderId);
        var productId = Guid.Empty;
        await UseUowAsync(async () =>
        {
            var order = await _orderRepository.GetAsync(specification);
            var orderLine = order!.OrderLines.First();
            productId = orderLine.ProductId;
            order.RemoveOrderLine(productId);
            await _orderRepository.UpdateAsync(order);
        });

        using var scope = _dataFilter.Disable<ISoftDelete>();
        var order = await _orderRepository.GetAsync(specification);

        order!.OrderLines.Should().Contain(v => v.ProductId == productId && v.IsDeleted);
    }

    [Fact]
    public async Task DbContextBase_RemoveCascadingObject_SoftDelete()
    {
        var specification = new OrderWithDetailsSpecification(FakeDataBuilder.OrderId);
        await UseUowAsync(async () =>
        {
            var order = await _orderRepository.GetAsync(specification);
            order!.Details = null;
            // PK property should be nullable.
            await _orderRepository.UpdateAsync(order);
        });

        using var scope = _dataFilter.Disable<ISoftDelete>();
        var order = await _orderRepository.GetAsync(specification);
        var details = order!.Details;

        details.Should().NotBeNull();
        details.Should().BeEquivalentTo(new { IsDeleted = true }, opts => opts.ExcludingFields());
    }

    [Fact]
    public async Task DbContextBase_RemovePrincpalEntity_KeepCascadingEntitiesState()
    {
        var specification = new OrderWithDetailsSpecification(FakeDataBuilder.OrderId);
        var productId = Guid.Empty;
        await UseUowAsync(async () =>
        {
            var order = await _orderRepository.GetAsync(specification);
            var orderLine = order!.OrderLines.First();
            productId = orderLine.ProductId;
            order.RemoveOrderLine(productId);
            await _orderRepository.DeleteAsync(order);
        });

        using var scope = _dataFilter.Disable<ISoftDelete>();
        var order = await _orderRepository.GetAsync(specification);

        order.Should().NotBeNull();
        order!.IsDeleted.Should().BeTrue();
        order.OrderLines.Should().Contain(v => v.ProductId == productId && v.IsDeleted);
        order.Details.Should().NotBeNull();
        order.Details!.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public async Task DbContextBase_QueryWithCascadingCollection_IgnoreDeletedEntity()
    {
        var specification = new OrderWithDetailsSpecification(FakeDataBuilder.OrderId);
        var productId = Guid.Empty;
        await UseUowAsync(async () =>
        {
            var order = await _orderRepository.GetAsync(specification);
            var orderLine = order!.OrderLines.First();
            productId = orderLine.ProductId;
            order.RemoveOrderLine(productId);
            await _orderRepository.UpdateAsync(order);
        });
        var order = await _orderRepository.GetAsync(specification);

        order!.OrderLines.Should()
                         .OnlyContain(v => !v.IsDeleted).And
                         .NotContain(v => v.ProductId == productId);
    }

    private async Task UseUowAsync(Func<Task> func)
    {
        var uowMgr = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        using var uow = uowMgr.Begin(requireNew: true);
        await func();
        await uow.CommitChangesAsync();
    }
}
