using System.Threading.Tasks;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data
{
    public class FakeDataBuilder
    {
        private readonly FakeDbContext _dbContext;

        public FakeDataBuilder(FakeDbContext dbContext)
        {
            _dbContext = dbContext;
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
                await _dbContext.AddAsync(book);
            _dbContext.SaveChanges();
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
                await _dbContext.AddAsync(author);
            _dbContext.SaveChanges();
        }
    }
}