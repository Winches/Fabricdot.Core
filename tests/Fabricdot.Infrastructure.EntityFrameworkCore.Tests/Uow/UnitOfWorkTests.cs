using Fabricdot.Infrastructure.Uow.Abstractions;
using Fabricdot.Test.Helpers.Domain.Aggregates.CustomerAggregate;
using Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Uow;

public class UnitOfWorkTests : EntityFrameworkCoreTestsBase
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;

    /// <inheritdoc />
    public UnitOfWorkTests()
    {
        _unitOfWorkManager = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        _orderRepository = ServiceProvider.GetRequiredService<IOrderRepository>();
        _customerRepository = ServiceProvider.GetRequiredService<ICustomerRepository>();
    }

    public static IEnumerable<object[]> GetUnitOfWorkOptions()
    {
        yield return new object[] { new UnitOfWorkOptions() };
        yield return new object[] { new UnitOfWorkOptions { IsTransactional = true } };
    }

    [Theory]
    [MemberData(nameof(GetUnitOfWorkOptions))]
    public async Task CommitChangesAsync_PerformDatabase_SaveChanges(UnitOfWorkOptions options)
    {
        var expected = Create<Order>();
        using (var uow1 = _unitOfWorkManager.Begin(options))
        {
            await _orderRepository.AddAsync(expected);
            await uow1.CommitChangesAsync();
        }

        var actual = await _orderRepository.GetByIdAsync(expected.Id);

        actual.Should().Be(expected);
    }

    [Theory]
    [MemberData(nameof(GetUnitOfWorkOptions))]
    public async Task CommitChangesAsync_WhenErrorOccurred_DiscardChanges(UnitOfWorkOptions options)
    {
        var order = Create<Order>();
        var exception = Create<Exception>();
        async Task UseUow()
        {
            using var _ = _unitOfWorkManager.Begin(options);
            await _orderRepository.AddAsync(order);
            throw exception;
        }

        await Awaiting(UseUow)
                           .Should()
                           .ThrowAsync<Exception>()
                           .WithMessage(exception.Message);

        var actual = await _orderRepository.GetByIdAsync(order.Id);

        actual.Should().BeNull();
    }

    [Theory]
    [MemberData(nameof(GetUnitOfWorkOptions))]
    public async Task Dispose_PerformDatabase_DiscardChanges(UnitOfWorkOptions options)
    {
        var order = Create<Order>();
        using (var uow = _unitOfWorkManager.Begin(options))
        {
            await _orderRepository.AddAsync(order);
        }
        var actual = await _orderRepository.GetByIdAsync(order.Id);

        actual.Should().BeNull();
    }

    [Fact]
    public async Task CommitChangesAsync_MultipleDbContext_SaveChanges()
    {
        var order = Create<Order>();
        var customer = Create<Customer>();
        using (var uow = _unitOfWorkManager.Begin(new UnitOfWorkOptions { IsTransactional = true }))
        {
            await _orderRepository.AddAsync(order);
            await _customerRepository.AddAsync(customer);
            await uow.CommitChangesAsync();
        }

        var actual1 = await _orderRepository.GetByIdAsync(order.Id);
        var actual2 = await _customerRepository.GetByIdAsync(customer.Id);

        actual1.Should().Be(order);
        actual2.Should().Be(customer);
    }
}