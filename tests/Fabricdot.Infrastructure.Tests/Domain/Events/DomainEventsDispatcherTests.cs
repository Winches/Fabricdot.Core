using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fabricdot.Domain.Auditing;
using Fabricdot.Domain.Entities;
using Fabricdot.Domain.Events;
using Fabricdot.Infrastructure.Domain.Events;
using MediatR;
using Moq;
using Xunit;

namespace Fabricdot.Infrastructure.Tests.Domain.Events;

public class DomainEventsDispatcherTests
{
    private readonly IPublisher _publisher;
    private readonly List<INotification> _notifications = new List<INotification>();

    public DomainEventsDispatcherTests()
    {
        var fakePublisher = new Mock<IPublisher>();

        Task Func(INotification notification, CancellationToken _)
        {
            _notifications.Add(notification);
            return Task.CompletedTask;
        }

        fakePublisher.Setup(v => v.Publish(It.IsAny<INotification>(), default))
            .Returns((Func<INotification, CancellationToken, Task>)Func);
        _publisher = fakePublisher.Object;
    }

    [Fact]
    public async Task DispatchEventsAsync_GivenNull_ThrowException()
    {
        var domainEventsDispatcher = new DomainEventsDispatcher(_publisher);

        async Task Func()
        {
            await domainEventsDispatcher.DispatchEventsAsync(null);
        }

        await Assert.ThrowsAsync<ArgumentNullException>(Func);
    }

    [Fact]
    public async Task DispatchEventsAsync_GivenChangeInfos_PublishDomainEventNotifications()
    {
        var domainEventsDispatcher = new DomainEventsDispatcher(_publisher);
        var changeInfos = GetEntityChangeInfos();
        var domainEvents = changeInfos.Select(v => v.Entity)
            .OfType<IHasDomainEvents>()
            .SelectMany(v => v.DomainEvents)
            .ToArray();

        await domainEventsDispatcher.DispatchEventsAsync(changeInfos);
        var domainEventNotifications = _notifications.OfType<DomainEventNotification>();
        Assert.Contains(domainEventNotifications, v => domainEvents.Contains(v.Event));
    }

    private static List<EntityChangeInfo> GetEntityChangeInfos()
    {
        var employee = new Employee("Jonathan", "Walker", "001");
        employee.AddDomainEvent(new EntityCreatedEvent<Employee>(employee));
        employee.AddDomainEvent(new EntityChangedEvent<Employee>(employee));

        var changeInfos = new List<EntityChangeInfo>
        {
            new(employee, EntityStatus.Created),
            new(new object(), EntityStatus.Created)
        };
        return changeInfos;
    }
}