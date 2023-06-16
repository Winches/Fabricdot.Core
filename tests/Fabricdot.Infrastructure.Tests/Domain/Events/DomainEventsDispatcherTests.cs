using Fabricdot.Domain.Auditing;
using Fabricdot.Domain.Entities;
using Fabricdot.Domain.Events;
using Fabricdot.Infrastructure.Domain.Events;
using MediatR;
using Moq;

namespace Fabricdot.Infrastructure.Tests.Domain.Events;

public class DomainEventsDispatcherTests : TestFor<DomainEventsDispatcher>
{
    private readonly List<INotification> _notifications = new();

    public DomainEventsDispatcherTests()
    {
        var publisheMock = InjectMock<IPublisher>();

        Task Func(INotification notification, CancellationToken _)
        {
            _notifications.Add(notification);
            return Task.CompletedTask;
        }

        publisheMock.Setup(v => v.Publish(It.IsAny<INotification>(), default))
            .Returns((Func<INotification, CancellationToken, Task>)Func);
    }

    [Fact]
    public async Task DispatchEventsAsync_GivenNull_ThrowException()
    {
        await Awaiting(() => Sut.DispatchEventsAsync(null!)).Should()
                                                           .ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task DispatchEventsAsync_GivenChangeInfos_PublishDomainEventNotifications()
    {
        var changeInfos = GetEntityChangeInfos();
        var domainEvents = changeInfos.Select(v => v.Entity)
                                      .OfType<IHasDomainEvents>()
                                      .SelectMany(v => v.DomainEvents)
                                      .ToArray();

        await Sut.DispatchEventsAsync(changeInfos);

        _notifications.OfType<DomainEventNotification>().Should().OnlyContain(v => domainEvents.Contains(v.Event));
    }

    private List<EntityChangeInfo> GetEntityChangeInfos()
    {
        var entity = Create<AggregateRoot<int>>();
        entity.AddDomainEvent(Create<IDomainEvent>());
        entity.AddDomainEvent(Create<IDomainEvent>());

        return new List<EntityChangeInfo>
        {
            new(entity, EntityStatus.Created),
            new(new object(), EntityStatus.Created)
        };
    }
}
