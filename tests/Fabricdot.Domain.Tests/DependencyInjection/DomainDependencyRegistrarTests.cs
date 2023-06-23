using Ardalis.Specification;
using Fabricdot.Domain.DependencyInjection;
using Fabricdot.Domain.Events;
using Fabricdot.Domain.Services;
using Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Domain.Tests.DependencyInjection;

public class DomainDependencyRegistrarTests : TestFor<DomainDependencyRegistrar>
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public override Task<Order> AddAsync(Order entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<long> CountAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<long> CountAsync(ISpecification<Order> specification, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task DeleteAsync(Order entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<Order?> GetAsync(ISpecification<Order> specification, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Order?> GetByIdAsync(Guid id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<Order?> GetBySpecAsync(ISpecification<Order> specification, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<IReadOnlyList<Order>> ListAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<IReadOnlyList<Order>> ListAsync(ISpecification<Order> specification, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Order>> ListAsync(IEnumerable<Guid> keys, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task UpdateAsync(Order entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }

    private class OrderPlacedDomainEventHandler : IDomainEventHandler<OrderPlacedDomainEvent>
    {
        public Task HandleAsync(OrderPlacedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    [AutoData]
    [Theory]
    public void Register_GivenDomainEventHandler_RegisterType(ServiceCollection services)
    {
        var implementationType = typeof(OrderPlacedDomainEventHandler);

        Sut.Register(services, implementationType);

        services.Should()
                .ContainSingle<IDomainEventHandler<OrderPlacedDomainEvent>>().Which.ImplementationType
                .Should()
                .Be(implementationType);
    }

    [AutoData]
    [Theory]
    public void Register_GivenDomainService_RegisterType(ServiceCollection services)
    {
        var implementationType = typeof(OrderNumberGenerator);

        Sut.Register(services, implementationType);

        services.Should()
                .ContainSingle<IOrderNumberGenerator>().Which.ImplementationType
                .Should()
                .Be(implementationType);
        services.Should().NotContain<IDomainService>();
    }

    [AutoData]
    [Theory]
    public void Register_GivenRepository_RegisterType(ServiceCollection services)
    {
        var implementationType = typeof(OrderRepository);
        var serviceTypes = new[] { implementationType, typeof(IOrderRepository), typeof(IReadOnlyRepository<Order, Guid>) };

        Sut.Register(services, implementationType);
        var serviceProvider = services.BuildServiceProvider();

        serviceTypes.ForEach(serviceType =>
        {
            services.Should()
                    .Contain(serviceType, ServiceLifetime.Scoped);
        });
        serviceTypes.ForEach(serviceType =>
        {
            serviceProvider.GetService(serviceType)
                           .Should()
                           .BeOfType(implementationType);
        });
    }
}
