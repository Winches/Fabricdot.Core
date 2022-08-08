using AutoFixture.Xunit2;
using Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Repositories;

public class EfRepository_AddAsync_Tests : EntityFrameworkCoreTestsBase
{
    private readonly IOrderRepository _orderRepository;

    public EfRepository_AddAsync_Tests()
    {
        _orderRepository = ServiceProvider.GetRequiredService<IOrderRepository>();
    }

    [AutoData]
    [Theory]
    public async Task AddAsync_GivenUnsavedEntity_Correctly(Order expected)
    {
        await _orderRepository.AddAsync(expected);

        var actual = await _orderRepository.GetByIdAsync(expected.Id);

        actual.Should().BeEquivalentTo(expected);
    }

    [AutoData]
    [Theory]
    public async Task AddAsync_GivenSavedEntity_ThrowException(Order order)
    {
        async Task TestCode()
        {
            await _orderRepository.AddAsync(order);
            await _orderRepository.AddAsync(order);
        }

        await Awaiting(TestCode).Should().ThrowAsync<DbUpdateException>();
    }

    [Fact]
    public async Task AddAsync_GivenNull_ThrowException()
    {
        await Awaiting(() => _orderRepository.AddAsync(null))
                           .Should()
                           .ThrowAsync<ArgumentNullException>();
    }
}