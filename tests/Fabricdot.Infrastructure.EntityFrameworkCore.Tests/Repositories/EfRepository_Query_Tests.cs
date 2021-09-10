using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Repositories
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class EfRepository_Query_Tests : EfRepositoryTestsBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly Author _softDeletedAuthor;

        public EfRepository_Query_Tests()
        {
            var provider = ServiceScope.ServiceProvider;
            _bookRepository = provider.GetRequiredService<IBookRepository>();
            _authorRepository = provider.GetRequiredService<IAuthorRepository>();

            _softDeletedAuthor = SoftDeleteAuthor();
        }

        [Theory]
        [InlineData("CSharp")]
        [InlineData(null)]
        public async Task GetByIdAsync_GivenId_ReturnEntity(string bookId)
        {
            var expected = await FakeDbContext.FindAsync<Book>(bookId);
            var actual = await _bookRepository.GetByIdAsync(bookId);
            Assert.Equal(expected, actual);
        }

        //private async Task<Author> GetSoftDeletedAuthorAsync()
        //{
        //    var author = await _authorRepository.GetByIdAsync(1);
        //    await _authorRepository.DeleteAsync(author);
        //    await UnitOfWork.CommitChangesAsync();
        //    return author;
        //}
        [Fact]
        public async Task GetByIdAsync_GivenSoftDeletedId_ReturnNull()
        {
            var author = _softDeletedAuthor;
            var actual = await _authorRepository.GetByIdAsync(author.Id);
            Assert.Null(actual);
        }

        [Theory]
        [InlineData("CSharp")]
        [InlineData(null)]
        public async Task GetBySpecAsync_GivenSpec_ReturnSpecificEntity(string bookName)
        {
            var specification = new BookFilterSpecification(bookName);
            var expected = await FakeDbContext.Set<Book>().FirstOrDefaultAsync(v => v.Name == bookName);
            var actual = await _bookRepository.GetBySpecAsync(specification);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetBySpecAsync_GivenSoftDeletedSpec_ReturnNull()
        {
            var author = _softDeletedAuthor;
            var specification = new AuthorFilterSpecification(author.LastName);
            var actual = await _authorRepository.GetBySpecAsync(specification);
            Assert.Null(actual);
        }

        [Theory]
        [InlineData("CSharp")]
        [InlineData(null)]
        public async Task ListAsync_GivenSpec_ReturnSpecificEntities(string bookName)
        {
            var specification = new BookFilterSpecification(bookName);
            var expected = await FakeDbContext.Set<Book>().Where(v => v.Name == bookName).ToListAsync();
            var actual = await _bookRepository.ListAsync(specification);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ListAsync_GivenSpec_ReturnSpecificEntitiesWithoutSoftDeleted()
        {
            var author = _softDeletedAuthor;
            var specification = new AuthorFilterSpecification(author.LastName);
            var actual = await _authorRepository.ListAsync(specification);
            Assert.Empty(actual);
        }

        [Fact]
        public async Task ListAllAsync_ReturnAllEntities()
        {
            var expected = await FakeDbContext.Set<Book>().ToListAsync();
            var actual = await _bookRepository.ListAllAsync();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ListAllAsync_ReturnAllEntitiesWithoutSoftDeleted()
        {
            var author = _softDeletedAuthor;
            var actual = await _authorRepository.ListAllAsync();
            Assert.DoesNotContain(author, actual);
        }

        [Theory]
        [InlineData("CSharp")]
        [InlineData(null)]
        public async Task CountAsync_GivenSpec_ReturnCorrectlyCount(string bookName)
        {
            var specification = new BookFilterSpecification(bookName);
            var expected = await FakeDbContext.Set<Book>().CountAsync(v => v.Name == bookName);
            var actual = await _bookRepository.CountAsync(specification);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task CountAsync_GivenSpec_ReturnCorrectlyCountWithoutSoftDeleted()
        {
            var author = _softDeletedAuthor;
            var specification = new AuthorFilterSpecification(author.LastName);
            var actual = await _authorRepository.CountAsync(specification);
            Assert.Equal(0, actual);
        }

        private Author SoftDeleteAuthor()
        {
            var author = FakeDbContext.Find<Author>(2);
            author.MarkDeleted();
            FakeDbContext.Update(author);
            FakeDbContext.SaveChanges();
            return author;
        }
    }
}