using System.Threading;
using System.Threading.Tasks;
using Fabricdot.Domain.Core.Entities;
using Fabricdot.Domain.Core.Events;
using Fabricdot.Infrastructure.Core.Domain.Events;
using Fabricdot.Test.Shared;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.Infrastructure.Core.Tests.Domain.Events
{
    public class DomainEventNotificationHandlerTests : IntegrationTestBase
    {
        internal class Employee : AggregateRootBase<int>
        {
            public string FirstName { get; private set; }

            public string LastName { get; private set; }

            public string Number { get; private set; }

            public Employee(
                string firstName,
                string lastName,
                string number)
            {
                FirstName = firstName;
                LastName = lastName;
                Number = number;
            }
        }

        internal class OrderCreatedEventHandler1 : IDomainEventHandler<EntityCreatedEvent<Employee>>
        {
            public static string FirstName { get; private set; }

            /// <inheritdoc />
            public Task HandleAsync(EntityCreatedEvent<Employee> domainEvent, CancellationToken cancellationToken)
            {
                var employee = domainEvent.Entity;
                FirstName = employee.FirstName;
                return Task.CompletedTask;
            }
        }

        internal class OrderCreatedEventHandler2 : IDomainEventHandler<EntityCreatedEvent<Employee>>
        {
            public static string LastName { get; private set; }

            /// <inheritdoc />
            public Task HandleAsync(EntityCreatedEvent<Employee> domainEvent, CancellationToken cancellationToken)
            {
                var employee = domainEvent.Entity;
                LastName = employee.LastName;
                return Task.CompletedTask;
            }
        }

        private readonly INotificationHandler<DomainEventNotification> _notificationHandler;

        /// <inheritdoc />
        public DomainEventNotificationHandlerTests()
        {
            _notificationHandler = ServiceProvider.GetRequiredService<INotificationHandler<DomainEventNotification>>();
        }

        /// <inheritdoc />
        protected override void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddTransient<INotificationHandler<DomainEventNotification>, DomainEventNotificationHandler>()
                .AddTransient<IDomainEventHandler<EntityCreatedEvent<Employee>>, OrderCreatedEventHandler1>()
                .AddTransient<IDomainEventHandler<EntityCreatedEvent<Employee>>, OrderCreatedEventHandler2>();
        }

        [Fact]
        public async Task Handle_SubscribeMultipleEventHandlers_TriggerAllHandlers()
        {
            var employee = new Employee("Allen", "Yeager", "1");
            var @event = new EntityCreatedEvent<Employee>(employee);
            await _notificationHandler.Handle(new DomainEventNotification(@event), default);

            Assert.Equal(employee.FirstName, OrderCreatedEventHandler1.FirstName);
            Assert.Equal(employee.LastName, OrderCreatedEventHandler2.LastName);
        }

        [Fact]
        public async Task Handle_NonSubscription_DoNothing()
        {
            var employee = new Employee("Allen", "Yeager", "1");
            var @event = new EntityDeletedEvent<Employee>(employee);
            await _notificationHandler.Handle(new DomainEventNotification(@event), default);
        }
    }
}