using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Repositories;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class EfRepository_DeleteAsync_Tests : EfRepositoryTestsBase
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;

    public EfRepository_DeleteAsync_Tests()
    {
        var provider = ServiceScope.ServiceProvider;
        _bookRepository = provider.GetRequiredService<IBookRepository>();
        _authorRepository = provider.GetRequiredService<IAuthorRepository>();
    }

    [Fact]
    public async Task DeleteAsync_GivenEntity_DeleteCorrectly()
    {
        var author = await _authorRepository.GetByIdAsync(1);
        await _authorRepository.DeleteAsync(author);

        var deletedAuthor = await _authorRepository.GetByIdAsync(author.Id);
        Assert.Null(deletedAuthor);
    }

    [Fact]
    public async Task DeleteAsync_GivenUnsavedEntity_ThrowException()
    {
        var book = new Book("10", "Python");

        async Task Func() => await _bookRepository.DeleteAsync(book);
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(Func);
    }

    [Fact]
    public async Task DeleteAsync_GivenNull_ThrowException()
    {
        async Task Func() => await _bookRepository.DeleteAsync(null);
        await Assert.ThrowsAsync<ArgumentNullException>(Func);
    }
}