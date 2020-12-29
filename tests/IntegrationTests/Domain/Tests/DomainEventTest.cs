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
            private readonly IBookRepository _bookRepository;

            public BookCreatedEventHandler(IBookRepository bookRepository)
            {
                _bookRepository = bookRepository;
            }

            /// <inheritdoc />
            public async Task Handle(EntityCreatedEvent<Book> notification, CancellationToken cancellationToken)
            {
                var book = new Book("1", $"{notification.Entity.Name}-I");
                await _bookRepository.AddAsync(book, cancellationToken);
            }
        }

        public class BookCreatedEvent2Handler : IDomainEventHandler<EntityCreatedEvent<Book>>
        {
            private readonly IBookRepository _bookRepository;

            public BookCreatedEvent2Handler(IBookRepository bookRepository)
            {
                _bookRepository = bookRepository;
            }

            /// <inheritdoc />
            public async Task Handle(EntityCreatedEvent<Book> notification, CancellationToken cancellationToken)
            {
                var book = new Book("2", $"{notification.Entity.Name}-II");
                await _bookRepository.AddAsync(book, cancellationToken);
            }
        }

        public class BookChangedEventA : EntityChangedEvent<Book>
        {
            /// <inheritdoc />
            public BookChangedEventA(Book entity) : base(entity)
            {
            }
        }

        public class BookChangedEventB : EntityChangedEvent<Book>
        {
            /// <inheritdoc />
            public BookChangedEventB(Book entity) : base(entity)
            {
            }
        }

        public class BookChangedEventAHandler : IDomainEventHandler<BookChangedEventA>
        {
            private readonly IBookRepository _bookRepository;

            public BookChangedEventAHandler(IBookRepository bookRepository)
            {
                _bookRepository = bookRepository;
            }

            /// <inheritdoc />
            public async Task Handle(BookChangedEventA notification, CancellationToken cancellationToken)
            {
                var book = new Book("1", $"{notification.Entity.Name}-I");
                await _bookRepository.AddAsync(book, cancellationToken);
            }
        }

        public class BookChangedEventBHandler : IDomainEventHandler<BookChangedEventB>
        {
            private readonly IBookRepository _bookRepository;

            public BookChangedEventBHandler(IBookRepository bookRepository)
            {
                _bookRepository = bookRepository;
            }

            /// <inheritdoc />
            public async Task Handle(BookChangedEventB notification, CancellationToken cancellationToken)
            {
                var book = new Book("2", $"{notification.Entity.Name}-II");
                await _bookRepository.AddAsync(book, cancellationToken);
            }
        }

        public DomainEventTest()
        {
            _bookRepository = ServiceScope.ServiceProvider.GetRequiredService<IBookRepository>();
        }

        [Fact]
        public async Task TestSubscribeMultipleEventHandlers()
        {
            var book = new Book("1000", "FSharp");
            await _bookRepository.AddAsync(book);
            book.AddDomainEvent(new EntityCreatedEvent<Book>(book));
            await UnitOfWork.CommitChangesAsync();

            var book1 = await _bookRepository.GetByNameAsync("FSharp-I");
            Assert.NotNull(book1);
            var book2 = await _bookRepository.GetByNameAsync("FSharp-II");
            Assert.NotNull(book2);
        }

        [Fact]
        public async Task TestPublishMultipleEvents()
        {
            var book = await _bookRepository.GetByNameAsync("Rust");
            book.ChangeCreatorId("99");
            book.AddDomainEvent(new BookChangedEventA(book));
            book.AddDomainEvent(new BookChangedEventB(book));
            await _bookRepository.UpdateAsync(book);
            await UnitOfWork.CommitChangesAsync();

            var book1 = await _bookRepository.GetByNameAsync("Rust-I");
            Assert.NotNull(book1);
            var book2 = await _bookRepository.GetByNameAsync("Rust-II");
            Assert.NotNull(book2);
        }
    }
}