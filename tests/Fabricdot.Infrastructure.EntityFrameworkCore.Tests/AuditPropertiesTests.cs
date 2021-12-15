using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Fabricdot.Domain.Events;
using Fabricdot.Domain.SharedKernel;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Repositories;
using Fabricdot.Infrastructure.Security;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class AuditPropertiesTests : EfRepositoryTestsBase
    {
        internal class AuthorCreatedEventHandler : IDomainEventHandler<EntityCreatedEvent<Author>>
        {
            private readonly IBookRepository _bookRepository;

            public AuthorCreatedEventHandler(IBookRepository bookRepository)
            {
                _bookRepository = bookRepository;
            }

            public async Task HandleAsync(EntityCreatedEvent<Author> domainEvent, CancellationToken cancellationToken)
            {
                var author = domainEvent.Entity;
                var book = new Book(author.Id.ToString(), $"{author.FirstName} Default Book.");
                await _bookRepository.AddAsync(book, cancellationToken);
            }
        }

        internal class AuthorChangedEventHandler : IDomainEventHandler<EntityChangedEvent<Author>>
        {
            private readonly IBookRepository _bookRepository;

            public AuthorChangedEventHandler(IBookRepository bookRepository)
            {
                _bookRepository = bookRepository;
            }

            public async Task HandleAsync(EntityChangedEvent<Author> domainEvent, CancellationToken cancellationToken)
            {
                //var author = domainEvent.Entity;
                await _bookRepository.GetByIdAsync("", cancellationToken);
            }
        }

        [Fact]
        public async Task SaveChangesAsync_CreateEntity_SetCreationProperties()
        {
            var author = new Author(100, "Martin", "Fowler");
            await FakeDbContext.AddAsync(author);
            await FakeDbContext.SaveChangesAsync();

            Assert.NotEqual(default, author.CreationTime);
            Assert.NotNull(author.CreatorId);
        }

        [Fact]
        public async Task SaveChangesAsync_CreateEntity_SetModificationProperties()
        {
            var author = new Author(100, "Martin", "Fowler");
            await FakeDbContext.AddAsync(author);
            await FakeDbContext.SaveChangesAsync();

            Assert.NotEqual(default, author.LastModificationTime);
            Assert.NotNull(author.LastModifierId);
        }

        [Fact]
        public async Task SaveChangesAsync_UpdateEntity_SetModificationProperties()
        {
            var author = await FakeDbContext.FindAsync<Author>(1);
            FakeDbContext.Update(author);
            var low = SystemClock.Now;
            await FakeDbContext.SaveChangesAsync();
            var high = SystemClock.Now;

            Assert.InRange(author.LastModificationTime, low, high);
            Assert.NotNull(author.LastModifierId);
        }

        [Fact]
        public async Task SaveChangesAsync_DeleteEntity_SetDeletionProperties()
        {
            var author = await FakeDbContext.FindAsync<Author>(1);
            FakeDbContext.Remove(author);
            await FakeDbContext.SaveChangesAsync();

            Assert.NotEqual(default, author.DeletionTime);
            Assert.NotNull(author.DeleterId);
        }

        [Fact]
        public async Task SaveChangesAsync_CreateEntityByDomainEvent_SetCreationProperties()
        {
            var author = new Author(100, "Martin", "Fowler");
            author.AddDomainEvent(new EntityCreatedEvent<Author>(author));
            await FakeDbContext.AddAsync(author);
            await FakeDbContext.SaveChangesAsync();

            var book = await FakeDbContext.FindAsync<Book>(author.Id.ToString());

            Assert.NotEqual(default, book.CreationTime);
            Assert.NotNull(book.CreatorId);
        }

        [Fact]
        public async Task SaveChangesAsync_UpdateEntityByDomainEvent_SetModificationProperties()
        {
            var author = new Author(100, "Martin", "Fowler");
            author.AddDomainEvent(new EntityCreatedEvent<Author>(author));
            await FakeDbContext.AddAsync(author);
            await FakeDbContext.SaveChangesAsync();

            var book = await FakeDbContext.FindAsync<Book>(author.Id.ToString());

            Assert.NotEqual(default, book.CreationTime);
            Assert.NotNull(book.CreatorId);
        }

        protected override void ConfigureServices(IServiceCollection serviceCollection)
        {
            base.ConfigureServices(serviceCollection);
            var mock = new Mock<ICurrentUser>();
            mock.SetupGet(v => v.Id).Returns("1");
            mock.SetupGet(v => v.UserName).Returns("Jason");
            serviceCollection.AddScoped(_ => mock.Object);
        }
    }
}