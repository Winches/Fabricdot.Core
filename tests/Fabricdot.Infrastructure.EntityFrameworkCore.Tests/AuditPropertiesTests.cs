using AutoFixture;
using Fabricdot.Domain.Events;
using Fabricdot.Domain.SharedKernel;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Fabricdot.Infrastructure.Security;
using Fabricdot.Infrastructure.Uow.Abstractions;
using Fabricdot.Test.Helpers.Domain.Aggregates.CustomerAggregate;
using Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;
using Fabricdot.Test.Helpers.Domain.Specifications;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests;

public class AuditPropertiesTests : EntityFrameworkCoreTestsBase
{
    internal class CustomerCreatedEventHandler : IDomainEventHandler<EntityCreatedEvent<Customer>>
    {
        private readonly IFixture _fixture;
        private readonly IOrderRepository _orderRepository;

        public CustomerCreatedEventHandler(
            IFixture fixture,
            IOrderRepository orderRepository)
        {
            _fixture = fixture;
            _orderRepository = orderRepository;
        }

        public async Task HandleAsync(
            EntityCreatedEvent<Customer> domainEvent,
            CancellationToken cancellationToken)
        {
            var customer = domainEvent.Entity;
            var order = new Order(Guid.NewGuid(), _fixture.Create<Address>(), customer.Id, null);
            await _orderRepository.AddAsync(order, cancellationToken);
        }
    }

    private readonly ICurrentUser _currentUser;
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;

    public AuditPropertiesTests()
    {
        _currentUser = ServiceProvider.GetService<ICurrentUser>();
        _orderRepository = ServiceProvider.GetService<IOrderRepository>();
        _customerRepository = ServiceProvider.GetService<ICustomerRepository>();
    }

    [Fact]
    public async Task SaveChangesAsync_CreateEntity_SetCreationProperties()
    {
        var expected = _currentUser.Id;
        var actual = Create<Order>();
        await _orderRepository.AddAsync(actual);

        actual.CreationTime.Should().NotBe(default);
        actual.CreatorId.Should().Be(expected);
    }

    [Fact]
    public async Task SaveChangesAsync_UpdateEntity_SetModificationProperties()
    {
        var expected = _currentUser.Id;
        var actual = await _orderRepository.GetByIdAsync(FakeDataBuilder.OrderId);
        var low = SystemClock.Now;
        await _orderRepository.UpdateAsync(actual);
        var high = SystemClock.Now;

        actual.LastModificationTime.Should().BeOnOrAfter(low).And.BeOnOrBefore(high);
        actual.LastModifierId.Should().Be(expected);
    }

    [Fact]
    public async Task SaveChangesAsync_DeleteEntity_SetDeletionProperties()
    {
        var expected = _currentUser.Id;
        var actual = await _orderRepository.GetByIdAsync(FakeDataBuilder.OrderId);
        await _orderRepository.DeleteAsync(actual);

        actual.DeletionTime.Should().NotBe(default);
        actual.DeleterId.Should().Be(expected);
    }

    [Fact]
    public async Task SaveChangesAsync_CreateEntityByDomainEvent_SetCreationProperties()
    {
        var uowMgr = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        using var uow = uowMgr.Begin();
        var expected = _currentUser.Id;
        var customer = Create<Customer>();
        customer.AddDomainEvent(new EntityCreatedEvent<Customer>(customer));
        await _customerRepository.AddAsync(customer);
        await uow.CommitChangesAsync();

        var actual = (await _orderRepository.ListAsync(new OrderFilterSpecification(customer.Id)))[0];

        actual.CreationTime.Should().NotBe(default);
        actual.CreatorId.Should().Be(expected);
    }

    protected override void ConfigureServices(IServiceCollection serviceCollection)
    {
        base.ConfigureServices(serviceCollection);
        var mock = Mock<ICurrentUser>();
        mock.SetupGet(v => v.Id).Returns(Create<string>());
        serviceCollection.AddScoped(_ => mock.Object);
    }
}