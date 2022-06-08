using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Fabricdot.Domain.Auditing;
using Fabricdot.Infrastructure.Data.Filters;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Repositories;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class EfRepository_Query_Tests : EfRepositoryTestsBase
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IDataFilter _dataFilter;

    public EfRepository_Query_Tests()
    {
        var provider = ServiceScope.ServiceProvider;
        _bookRepository = provider.GetRequiredService<IBookRepository>();
        _authorRepository = provider.GetRequiredService<IAuthorRepository>();
        _dataFilter = provider.GetRequiredService<IDataFilter>();
    }

    [Theory]
    [InlineData("1")]
    [InlineData(null)]
    public async Task GetByIdAsync_GivenId_ReturnEntity(string bookId)
    {
        var actual = await _bookRepository.GetByIdAsync(bookId);
        Assert.Equal(bookId, actual?.Id);
    }

    [Fact]
    public async Task GetByIdAsync_GivenSoftDeletedId_ReturnNull()
    {
        var actual = await _authorRepository.GetByIdAsync(FakeDataBuilder.DeletedAuthorId);
        Assert.Null(actual);
    }

    [Theory]
    [InlineData("CSharp")]
    [InlineData(null)]
    public async Task GetBySpecAsync_GivenSpec_ReturnSpecificEntity(string bookName)
    {
        var specification = new BookFilterSpecification(bookName);
        var expected = await _bookRepository.GetByNameAsync(bookName);
        var actual = await _bookRepository.GetBySpecAsync(specification);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetBySpecAsync_GivenSoftDeletedSpec_ReturnNull()
    {
        var specification = new AuthorFilterSpecification(FakeDataBuilder.DeletedAuthorId);
        var actual = await _authorRepository.GetBySpecAsync(specification);
        Assert.Null(actual);
    }

    [Theory]
    [InlineData("CSharp")]
    [InlineData(null)]
    public async Task ListAsync_GivenSpec_ReturnSpecificEntities(string bookName)
    {
        var specification = new BookFilterSpecification(bookName);
        var expected = await _bookRepository.GetByNameAsync(bookName);
        var actual = (await _bookRepository.ListAsync(specification)).SingleOrDefault();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task ListAsync_GivenSpec_ReturnSpecificEntitiesWithoutSoftDeleted()
    {
        var specification = new AuthorFilterSpecification(FakeDataBuilder.DeletedAuthorId);
        var actual = await _authorRepository.ListAsync(specification);
        Assert.Empty(actual);
    }

    [Fact]
    public async Task ListAsync_ReturnAllEntities()
    {
        var actual = await _bookRepository.ListAsync();
        Assert.NotEmpty(actual);
    }

    [Fact]
    public async Task ListAsync_ReturnAllEntitiesWithoutSoftDeleted()
    {
        var actual = await _authorRepository.ListAsync();
        Assert.DoesNotContain(actual, v => v.Id == FakeDataBuilder.DeletedAuthorId);
    }

    [Theory]
    [InlineData("CSharp")]
    [InlineData("Java")]
    public async Task CountAsync_GivenSpec_ReturnCorrectlyCount(string bookName)
    {
        var specification = new BookFilterSpecification(bookName);
        var expected = 1;
        var actual = await _bookRepository.CountAsync(specification);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task CountAsync_GivenSpec_ReturnCorrectlyCountWithoutSoftDeleted()
    {
        var specification = new AuthorFilterSpecification(FakeDataBuilder.DeletedAuthorId);
        var actual = await _authorRepository.CountAsync(specification);
        Assert.Equal(0, actual);
    }

    private async Task<Author> GetDeletedAuthorAsync()
    {
        using var scope = _dataFilter.Disable<ISoftDelete>();
        return await _authorRepository.GetByIdAsync(FakeDataBuilder.DeletedAuthorId);
    }
}