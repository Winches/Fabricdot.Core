using AutoFixture.Xunit2;
using Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Repositories;

public class EfRepository_UpdateAsync_Tests : EntityFrameworkCoreTestsBase
{
    private readonly IOrderRepository _orderRepository;

    public EfRepository_UpdateAsync_Tests()
    {
        _orderRepository = ServiceProvider.GetRequiredService<IOrderRepository>();
    }

    [Fact]
    public async Task UpdateAsync_GivenSavedEntity_SaveChanges()
    {
        var expected = (await _orderRepository.ListAsync())[0];
        expected.AddOrderLine(Fixture);
        await _orderRepository.UpdateAsync(expected);
        var order = await _orderRepository.GetByIdAsync(expected.Id);

        order.Should().BeEquivalentTo(expected);
    }

    [AutoData]
    [Theory]
    public async Task UpdateAsync_GivenUnsavedEntity_ThrowException(Order order)
    {
        await Awaiting(() => _orderRepository.UpdateAsync(order))
                           .Should()
                           .ThrowAsync<DbUpdateException>();
    }

    [Fact]
    public async Task UpdateAsync_GivenNull_ThrowException()
    {
        await Awaiting(() => _orderRepository.UpdateAsync(null))
                           .Should()
                           .ThrowAsync<ArgumentNullException>();
    }
}