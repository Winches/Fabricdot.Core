using Fabricdot.Infrastructure.Domain.Services;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;
using Fabricdot.Test.Helpers.Domain.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests;

public class ConcurrencyStampTest : EntityFrameworkCoreTestsBase
{
    private readonly IOrderRepository _orderRepository;

    public ConcurrencyStampTest()
    {
        _orderRepository = ServiceProvider.GetRequiredService<IOrderRepository>();
    }

    [Fact]
    public async Task UpdateAsync_WhenEntityChanged_ThrowException()
    {
        var specification = new OrderWithDetailsSpecification(FakeDataBuilder.OrderId);
        var order1 = await _orderRepository.GetAsync(specification);
        order1.AddOrderLine(Fixture);

        var order2 = await _orderRepository.GetAsync(specification);
        await _orderRepository.UpdateAsync(order2);

        await FluentActions.Awaiting(() => _orderRepository.UpdateAsync(order1))
                           .Should()
                           .ThrowAsync<DbUpdateConcurrencyException>();
    }

    [Fact]
    public async Task DeleteAsync_WhenEntityChanged_ThrowException()
    {
        var specification = new OrderWithDetailsSpecification(FakeDataBuilder.OrderId);
        var order1 = await _orderRepository.GetAsync(specification);

        var order2 = await _orderRepository.GetAsync(specification);
        order2.AddOrderLine(Fixture);
        await _orderRepository.UpdateAsync(order2);

        await FluentActions.Awaiting(() => _orderRepository.HardDeleteAsync(order1))
                           .Should()
                           .ThrowAsync<DbUpdateConcurrencyException>();
    }
}
