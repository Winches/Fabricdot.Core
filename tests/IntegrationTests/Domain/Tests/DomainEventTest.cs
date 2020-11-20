using System.Threading;
using System.Threading.Tasks;
using Fabricdot.Domain.Core.Events;
using Fabricdot.Infrastructure.Core.Data;
using IntegrationTests.Data;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace IntegrationTests.Domain.Tests
{
    public class DomainEventTest
    {
        private readonly IFakeRepository _fakeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public class FakeEntityCreatedEventHandler : IDomainEventHandler<EntityCreatedEvent<FakeEntity>>
        {
            /// <inheritdoc />
            public Task Handle(EntityCreatedEvent<FakeEntity> notification, CancellationToken cancellationToken)
            {
                notification.Entity.ChangeName("NewTestDispatchEvents");
                return Task.CompletedTask;
            }
        }

        public DomainEventTest()
        {
            var provider = ContainerBuilder.GetServiceProvider();
            provider.GetRequiredService<FakeDbContext>().Database.EnsureCreated();
            _fakeRepository = provider.GetRequiredService<IFakeRepository>();
            _unitOfWork = provider.GetRequiredService<IUnitOfWork>();
        }

        [Fact]
        public async Task TestDispatchEvents()
        {
            const string id = "D5734D1E-7687-4185-9AAD-F2CA94D2B00C";
            var name = "TestDispatchEvents";
            var entity = new FakeEntity(id, name);
            entity.AddDomainEvent(new EntityCreatedEvent<FakeEntity>(entity));
            await _fakeRepository.AddAsync(entity);
            await _unitOfWork.CommitChangesAsync();

            var newEntity = await _fakeRepository.GetByIdAsync(id);
            Assert.NotEqual(name, newEntity.Name);
        }
    }
}