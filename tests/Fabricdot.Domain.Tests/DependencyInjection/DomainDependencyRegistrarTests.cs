using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;
using Fabricdot.Domain.DependencyInjection;
using Fabricdot.Domain.Entities;
using Fabricdot.Domain.Events;
using Fabricdot.Domain.Services;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.Domain.Tests.DependencyInjection;

public class DomainDependencyRegistrarTests
{
    public class Order : AggregateRoot<Guid>
    {
    }

    private class OrderDomainService : IOrderDomainService
    {
    }

    private class OrderCreatedDomainEvent : DomainEventBase
    {
    }

    private class OrderCreatedDomainEventHandler : IDomainEventHandler<OrderCreatedDomainEvent>
    {
        public Task HandleAsync(OrderCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    private class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public override Task<Order> AddAsync(Order entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<long> CountAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<int> CountAsync(ISpecification<Order> specification, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task DeleteAsync(Order entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<Order> GetBySpecAsync(ISpecification<Order> specification, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<IReadOnlyList<Order>> ListAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<IReadOnlyList<Order>> ListAsync(ISpecification<Order> specification, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task UpdateAsync(Order entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }

    public interface IOrderRepository : IRepository<Order, Guid>
    {
    }

    internal interface IOrderDomainService : IDomainService
    {
    }

    [Fact]
    public void Register_GivenDomainEventHandler_RegisterType()
    {
        var services = new ServiceCollection();
        var registrar = new DomainDependencyRegistrar();
        var implementationType = typeof(OrderCreatedDomainEventHandler);
        var serviceType = typeof(IDomainEventHandler<OrderCreatedDomainEvent>);

        registrar.Register(services, implementationType);

        services.Should().ContainSingle(v => v.ServiceType == serviceType && v.ImplementationType == implementationType);
    }

    [Fact]
    public void Register_GivenDomainService_RegisterType()
    {
        var services = new ServiceCollection();
        var registrar = new DomainDependencyRegistrar();
        var implementationType = typeof(OrderDomainService);
        var serviceType = typeof(IOrderDomainService);

        registrar.Register(services, implementationType);

        services.Should().ContainSingle(v => v.ServiceType == serviceType && v.ImplementationType == implementationType);
        services.Should().NotContain(v => v.ServiceType == typeof(IDomainService));
    }

    [Fact]
    public void Register_GivenRepository_RegisterType()
    {
        var services = new ServiceCollection();
        var registrar = new DomainDependencyRegistrar();
        var implementationType = typeof(OrderRepository);
        var serviceTypes = new[] { typeof(IOrderRepository), typeof(IReadOnlyRepository<Order, Guid>) };

        registrar.Register(services, implementationType);

        services.Should().ContainSingle(v => serviceTypes.Contains(v.ServiceType) && v.ImplementationType == implementationType);
        services.Should().NotContain(v => v.ServiceType == typeof(IRepository));
    }
}