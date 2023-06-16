using AutoFixture.Xunit2;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Repositories;

public class EfRepository_DeleteAsync_Tests : EntityFrameworkCoreTestsBase
{
    private readonly IOrderRepository _orderRepository;

    public EfRepository_DeleteAsync_Tests()
    {
        _orderRepository = ServiceProvider.GetRequiredService<IOrderRepository>();
    }

    [Fact]
    public async Task DeleteAsync_GivenEntity_DeleteCorrectly()
    {
        var orderId = FakeDataBuilder.OrderId;
        var order = await _orderRepository.GetByIdAsync(orderId);
        await _orderRepository.DeleteAsync(order!);

        var deletedAuthor = await _orderRepository.GetByIdAsync(orderId);

        deletedAuthor.Should().BeNull();
    }

    [AutoData]
    [Theory]
    public async Task DeleteAsync_GivenUnsavedEntity_ThrowException(Order order)
    {
        await Awaiting(() => _orderRepository.DeleteAsync(order))
                           .Should()
                           .ThrowAsync<DbUpdateConcurrencyException>();
    }

    [Fact]
    public async Task DeleteAsync_GivenNull_ThrowException()
    {
        await Awaiting(() => _orderRepository.DeleteAsync(null!))
                           .Should()
                           .ThrowAsync<ArgumentNullException>();
    }
}
