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
    public class AuditPropertiesTests : EntityFrameworkCoreTestsBase
    {
        internal class AuthorCreatedEventHandler : IDomainEventHandler<EntityCreatedEvent<Author>>
        {
            private readonly IBookRepository _bookRepository;

            public AuthorCreatedEventHandler(IBookRepository bookRepository)
            {
                _bookRepository = bookRepository;
            }

            public async Task HandleAsync(
                EntityCreatedEvent<Author> domainEvent,
                CancellationToken cancellationToken)
            {
                var author = domainEvent.Entity;
                var book = new Book(author.Id.ToString(), $"{author.FirstName} Default Book.");
                await _bookRepository.AddAsync(book, cancellationToken);
            }
        }

        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;
        private static string CurrentUserId { get; } = "1";
        private static string CurrentUserName { get; } = "Jason";

        public AuditPropertiesTests()
        {
            var serviceProvider = ServiceScope.ServiceProvider;
            _authorRepository = serviceProvider.GetService<IAuthorRepository>();
            _bookRepository = serviceProvider.GetService<IBookRepository>();
        }

        [Fact]
        public async Task SaveChangesAsync_CreateEntity_SetCreationProperties()
        {
            var author = new Author(100, "Martin", "Fowler");
            await _authorRepository.AddAsync(author);

            Assert.NotEqual(default, author.CreationTime);
            Assert.Equal(CurrentUserId, author.CreatorId);
        }

        [Fact]
        public async Task SaveChangesAsync_CreateEntity_SetModificationProperties()
        {
            var author = new Author(100, "Martin", "Fowler");
            await _authorRepository.AddAsync(author);

            Assert.NotEqual(default, author.LastModificationTime);
            Assert.Equal(CurrentUserId, author.LastModifierId);
        }

        [Fact]
        public async Task SaveChangesAsync_UpdateEntity_SetModificationProperties()
        {
            var author = await _authorRepository.GetByIdAsync(1);
            var low = SystemClock.Now;
            await _authorRepository.UpdateAsync(author);
            var high = SystemClock.Now;

            Assert.InRange(author.LastModificationTime, low, high);
            Assert.Equal(CurrentUserId, author.LastModifierId);
        }

        [Fact]
        public async Task SaveChangesAsync_DeleteEntity_SetDeletionProperties()
        {
            var author = await _authorRepository.GetByIdAsync(1);
            await _authorRepository.DeleteAsync(author);

            Assert.NotEqual(default, author.DeletionTime);
            Assert.Equal(CurrentUserId, author.DeleterId);
        }

        [Fact]
        public async Task SaveChangesAsync_CreateEntityByDomainEvent_SetCreationProperties()
        {
            var author = new Author(100, "Martin", "Fowler");
            author.AddDomainEvent(new EntityCreatedEvent<Author>(author));
            await _authorRepository.AddAsync(author);

            var book = await _bookRepository.GetByIdAsync(author.Id.ToString());

            Assert.NotEqual(default, book.CreationTime);
            Assert.Equal(CurrentUserId, book.CreatorId);
        }

        protected override void ConfigureServices(IServiceCollection serviceCollection)
        {
            base.ConfigureServices(serviceCollection);
            var mock = new Mock<ICurrentUser>();
            mock.SetupGet(v => v.Id).Returns(CurrentUserId);
            mock.SetupGet(v => v.UserName).Returns(CurrentUserName);
            serviceCollection.AddScoped(_ => mock.Object);
        }
    }
}