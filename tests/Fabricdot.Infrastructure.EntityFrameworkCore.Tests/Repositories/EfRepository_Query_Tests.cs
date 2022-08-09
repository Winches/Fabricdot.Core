using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Fabricdot.Test.Helpers.Domain.Aggregates.CustomerAggregate;
using Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;
using Fabricdot.Test.Helpers.Domain.Specifications;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Repositories;

public class EfRepository_Query_Tests : EntityFrameworkCoreTestsBase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IOrderRepository _orderRepository;

    public EfRepository_Query_Tests()
    {
        _customerRepository = ServiceProvider.GetRequiredService<ICustomerRepository>();
        _orderRepository = ServiceProvider.GetRequiredService<IOrderRepository>();
    }

    [Fact]
    public async Task GetByIdAsync_GivenId_ReturnEntity()
    {
        var expected = FakeDataBuilder.OrderId;
        var actual = await _orderRepository.GetByIdAsync(expected);

        actual.Id.Should().Be(expected);
    }

    [Fact]
    public async Task GetByIdAsync_GivenNull_Throw()
    {
        await Awaiting(() => _customerRepository.GetByIdAsync(null))
                           .Should()
                           .ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task GetByIdAsync_GivenSoftDeletedId_ReturnNull()
    {
        var actual = await _orderRepository.GetByIdAsync(FakeDataBuilder.DeletedOrderId);

        actual.Should().BeNull();
    }

    [Fact]
    public async Task GetBySpecAsync_GivenSpec_ReturnSpecificEntity()
    {
        var orderId = FakeDataBuilder.OrderId;
        var expected = await _orderRepository.GetByIdAsync(orderId);
        var specification = new OrderWithDetailsSpecification(orderId);
        var actual = await _orderRepository.GetBySpecAsync(specification);

        actual.Should().Be(expected);
        actual.OrderLines.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetBySpecAsync_GivenNull_Throw()
    {
        await Awaiting(() => _orderRepository.GetBySpecAsync(null))
                           .Should()
                           .ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task GetBySpecAsync_GivenSoftDeletedSpec_ReturnNull()
    {
        var specification = new OrderWithDetailsSpecification(FakeDataBuilder.DeletedOrderId);
        var actual = await _orderRepository.GetBySpecAsync(specification);

        actual.Should().BeNull();
    }

    [Fact]
    public async Task ListAsync_GivenSpec_ReturnSpecificEntities()
    {
        var orderId = FakeDataBuilder.OrderId;
        var expected = await _orderRepository.GetByIdAsync(orderId);
        var specification = new OrderWithDetailsSpecification(orderId);
        var actual = await _orderRepository.ListAsync(specification);

        actual.Should().ContainSingle(expected);
    }

    [Fact]
    public async Task ListAsync_GivenNull_Throw()
    {
        await Awaiting(() => _orderRepository.ListAsync(null))
                           .Should()
                           .ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ListAsync_GivenSpec_ReturnSpecificEntitiesWithoutSoftDeleted()
    {
        var specification = new OrderWithDetailsSpecification(FakeDataBuilder.DeletedOrderId);
        var actual = await _orderRepository.ListAsync(specification);

        actual.Should().BeEmpty();
    }

    [Fact]
    public async Task ListAsync_ReturnAllEntities()
    {
        var actual = await _orderRepository.ListAsync();

        actual.Should().NotBeEmpty();
    }

    [Fact]
    public async Task ListAsync_ReturnAllEntitiesWithoutSoftDeleted()
    {
        var actual = await _orderRepository.ListAsync();

        actual.Should().NotContain(v => v.Id == FakeDataBuilder.DeletedOrderId);
    }

    [Fact]
    public async Task CountAsync_GivenSpec_ReturnCorrectlyCount()
    {
        var specification = new OrderWithDetailsSpecification(FakeDataBuilder.OrderId);
        const int expected = 1;
        var actual = await _orderRepository.CountAsync(specification);

        actual.Should().Be(expected);
    }

    [Fact]
    public async Task CountAsync_GivenSpec_ReturnCorrectlyCountWithoutSoftDeleted()
    {
        var specification = new OrderWithDetailsSpecification(FakeDataBuilder.DeletedOrderId);
        var actual = await _orderRepository.CountAsync(specification);

        actual.Should().Be(0);
    }
}