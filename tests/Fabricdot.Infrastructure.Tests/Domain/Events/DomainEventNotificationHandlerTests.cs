using System.Threading;
using System.Threading.Tasks;
using Fabricdot.Domain.Events;
using Fabricdot.Infrastructure.Domain.Events;
using Fabricdot.Test.Shared;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.Infrastructure.Tests.Domain.Events
{
    public class DomainEventNotificationHandlerTests : IntegrationTestBase
    {
        internal class EmployeeCreatedEventHandler1 : IDomainEventHandler<EntityCreatedEvent<Employee>>
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

        internal class EmployeeCreatedEventHandler2 : IDomainEventHandler<EntityCreatedEvent<Employee>>
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

        [Fact]
        public async Task Handle_SubscribeMultipleEventHandlers_TriggerAllHandlers()
        {
            var employee = new Employee("Allen", "Yeager", "1");
            var @event = new EntityCreatedEvent<Employee>(employee);
            await _notificationHandler.Handle(new DomainEventNotification(@event), default);

            Assert.Equal(employee.FirstName, EmployeeCreatedEventHandler1.FirstName);
            Assert.Equal(employee.LastName, EmployeeCreatedEventHandler2.LastName);
        }

        [Fact]
        public async Task Handle_NonSubscription_DoNothing()
        {
            var employee = new Employee("Allen", "Yeager", "1");
            var @event = new EntityRemovedEvent<Employee>(employee);
            await _notificationHandler.Handle(new DomainEventNotification(@event), default);
        }

        /// <inheritdoc />
        protected override void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddTransient<INotificationHandler<DomainEventNotification>, DomainEventNotificationHandler>()
                .AddTransient<IDomainEventHandler<EntityCreatedEvent<Employee>>, EmployeeCreatedEventHandler1>()
                .AddTransient<IDomainEventHandler<EntityCreatedEvent<Employee>>, EmployeeCreatedEventHandler2>();
        }
    }
}