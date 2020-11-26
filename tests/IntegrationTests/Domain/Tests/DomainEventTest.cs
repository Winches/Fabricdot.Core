using System.Threading;
using System.Threading.Tasks;
using Fabricdot.Domain.Core.Events;
using IntegrationTests.Data.Entities;
using IntegrationTests.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace IntegrationTests.Domain.Tests
{
    public class DomainEventTest : TestBase
    {
        private readonly IBookRepository _bookRepository;

        public class BookCreatedEventHandler : IDomainEventHandler<EntityCreatedEvent<Book>>
        {
            /// <inheritdoc />
            public Task Handle(EntityCreatedEvent<Book> notification, CancellationToken cancellationToken)
            {
                notification.Entity.ChangeName("CSharpV2");
                return Task.CompletedTask;
            }
        }

        public DomainEventTest()
        {
            _bookRepository = ServiceScope.ServiceProvider.GetRequiredService<IBookRepository>();
        }

        [Fact]
        public async Task TestDispatchEvents()
        {
            var entity = await _bookRepository.GetByNameAsync("CSharp");
            entity.AddDomainEvent(new EntityCreatedEvent<Book>(entity));
            await _bookRepository.UpdateAsync(entity);
            await UnitOfWork.CommitChangesAsync();

            var newEntity = await _bookRepository.GetByNameAsync("CSharp");
            Assert.Null(newEntity);
        }
    }
}