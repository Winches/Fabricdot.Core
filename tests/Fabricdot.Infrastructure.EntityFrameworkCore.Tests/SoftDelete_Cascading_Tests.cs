using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Fabricdot.Domain.Auditing;
using Fabricdot.Infrastructure.Data.Filters;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Repositories;
using Fabricdot.Infrastructure.Uow.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class SoftDelete_Cascading_Tests : EntityFrameworkCoreTestsBase
{
    private readonly IDataFilter _dataFilter;
    private readonly IBookRepository _bookRepository;

    public SoftDelete_Cascading_Tests()
    {
        var provider = ServiceScope.ServiceProvider;
        _dataFilter = provider.GetRequiredService<IDataFilter>();
        _bookRepository = provider.GetRequiredService<IBookRepository>();
    }

    [Fact]
    public async Task DbContextBase_RemoveEntityOfCascadingCollection_SoftDelete()
    {
        var specification = new BookFilterSpecification(FakeDataBuilder.BookWithTagsId, true);
        string tagName = null;
        await UseUowAsync(async () =>
        {
            var book = await _bookRepository.GetBySpecAsync(specification);
            tagName = book.Tags.First().Name;
            book.RemoveTag(tagName);
            await _bookRepository.UpdateAsync(book);
        });

        using var scope = _dataFilter.Disable<ISoftDelete>();
        var retrievalBook = await _bookRepository.GetBySpecAsync(specification);
        var tag = retrievalBook.Tags.SingleOrDefault(v => v.Name == tagName);
        Assert.NotNull(tag);
        Assert.True(tag.IsDeleted);
    }

    [Fact]
    public async Task DbContextBase_RemoveCascadingObject_SoftDelete()
    {
        var specification = new BookFilterSpecification(FakeDataBuilder.BookWithTagsId, true);
        await UseUowAsync(async () =>
        {
            var book = await _bookRepository.GetBySpecAsync(specification);
            book.Contents = null;
            await _bookRepository.UpdateAsync(book);
        });

        using var scope = _dataFilter.Disable<ISoftDelete>();
        var retrievalBook = await _bookRepository.GetBySpecAsync(specification);
        var contents = retrievalBook.Contents;
        Assert.NotNull(contents);
        Assert.True(contents.IsDeleted);
    }

    [Fact]
    public async Task DbContextBase_RemovePrincpalEntity_KeepCascadingEntitiesState()
    {
        var specification = new BookFilterSpecification(FakeDataBuilder.BookWithTagsId, true);
        string tagName = null;
        await UseUowAsync(async () =>
        {
            var book = await _bookRepository.GetBySpecAsync(specification);
            tagName = book.Tags.First().Name;
            book.RemoveTag(tagName);
            await _bookRepository.DeleteAsync(book);
        });

        using var scope = _dataFilter.Disable<ISoftDelete>();
        var retrievalBook = await _bookRepository.GetBySpecAsync(specification);
        var tag = retrievalBook.Tags.SingleOrDefault(v => v.Name == tagName);
        var contents = retrievalBook.Contents;

        Assert.NotNull(retrievalBook);
        Assert.True(retrievalBook.IsDeleted);
        Assert.True(tag.IsDeleted);
        Assert.NotNull(contents);
        Assert.False(contents.IsDeleted);
    }

    [Fact]
    public async Task DbContextBase_QueryWithCascadingCollection_IgnoreDeletedEntity()
    {
        var specification = new BookFilterSpecification(FakeDataBuilder.BookWithTagsId, true);
        using var scope = _dataFilter.Enable<ISoftDelete>();
        var book = await _bookRepository.GetBySpecAsync(specification);
        Assert.DoesNotContain(book.Tags, v => v.Name == FakeDataBuilder.DeletedBookTag);
    }

    private async Task UseUowAsync(Func<Task> func)
    {
        var uowMgr = ServiceScope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        using var uow = uowMgr.Begin(requireNew: true);
        await func();
        await uow.CommitChangesAsync();
    }
}