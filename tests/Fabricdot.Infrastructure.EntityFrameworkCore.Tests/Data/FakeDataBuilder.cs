using System.Threading.Tasks;
using Fabricdot.Infrastructure.Core.Data;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Repositories;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data
{
    public class FakeDataBuilder
    {
        private readonly FakeDbContext _dbContext;
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IUnitOfWork _unitOfWork;

        public FakeDataBuilder(
            FakeDbContext dbContext,
            IBookRepository bookRepository,
            IAuthorRepository authorRepository,
            IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task BuildAsync()
        {
            await _dbContext.Database.EnsureCreatedAsync();
            await AddBooks();
            await AddAuthors();
        }

        private async Task AddBooks()
        {
            var books = new[]
            {
                new Book("1", "CSharp"),
                new Book("2", "Java"),
                new Book("3", "Typescript"),
                new Book("4", "Rust"),
                new Book("5", "CPP")
            };
            foreach (var book in books)
                await _bookRepository.AddAsync(book);
            await _unitOfWork.CommitChangesAsync();
        }

        private async Task AddAuthors()
        {
            var authors = new[]
            {
                new Author(1, "Anders", "Hejlsberg"),
                new Author(2, "James", "Gosling"),
                new Author(3, "Bjarne", "Stroustrup"),
                new Author(4, "Graydon", "Hoare")
            };
            foreach (var author in authors)
                await _authorRepository.AddAsync(author);
            await _unitOfWork.CommitChangesAsync();
        }
    }
}