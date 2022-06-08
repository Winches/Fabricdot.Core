using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Repositories;
using Fabricdot.Infrastructure.Uow.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Uow;

public class UnitOfWorkTests : EntityFrameworkCoreTestsBase
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly IBookRepository _bookRepository;
    private readonly IPublisherRepository _publisherRepository;

    /// <inheritdoc />
    public UnitOfWorkTests()
    {
        var provider = ServiceScope.ServiceProvider;
        _unitOfWorkManager = provider.GetRequiredService<IUnitOfWorkManager>();
        _bookRepository = provider.GetRequiredService<IBookRepository>();
        _publisherRepository = provider.GetRequiredService<IPublisherRepository>();
    }

    public static IEnumerable<object[]> GetUnitOfWorkOptions()
    {
        yield return new object[] { new UnitOfWorkOptions() };
        yield return new object[] { new UnitOfWorkOptions { IsTransactional = true } };
    }

    [Theory]
    [MemberData(nameof(GetUnitOfWorkOptions))]
    public async Task CommitChangesAsync_PerformDatabase_SaveChanges(UnitOfWorkOptions options)
    {
        const string bookId = "9";
        var book = new Book(bookId, "Python");
        using (var uow1 = _unitOfWorkManager.Begin(options))
        {
            await _bookRepository.AddAsync(book);
            await uow1.CommitChangesAsync();
        }

        var retrievalBook = await _bookRepository.GetByIdAsync(bookId);
        Assert.NotNull(retrievalBook);
    }

    [Theory]
    [MemberData(nameof(GetUnitOfWorkOptions))]
    public async Task CommitChangesAsync_WhenErrorOccurred_DiscardChanges(UnitOfWorkOptions options)
    {
        const string bookId = "10";
        var book = new Book(bookId, "Go");
        var exception = new Exception("Something happened.");
        async Task UseUow()
        {
            using var _ = _unitOfWorkManager.Begin(options);
            await _bookRepository.AddAsync(book);
            throw exception;
        }

        var recordException = await Record.ExceptionAsync(UseUow);
        Assert.Same(exception, recordException);

        var retrievalBook = await _bookRepository.GetByIdAsync(bookId);
        Assert.Null(retrievalBook);
    }

    [Theory]
    [MemberData(nameof(GetUnitOfWorkOptions))]
    public async Task Dispose_PerformDatabase_DiscardChanges(UnitOfWorkOptions options)
    {
        const string bookId = "9";
        var book = new Book(bookId, "Python");
        using (var uow = _unitOfWorkManager.Begin(options))
        {
            await _bookRepository.AddAsync(book);
        }
        var retrievalBook = await _bookRepository.GetByIdAsync(bookId);
        Assert.Null(retrievalBook);
    }

    [Fact]
    public async Task CommitChangesAsync_MultipleDbContext_SaveChanges()
    {
        const string bookId = "10";
        const string publisherId = "1";
        var book = new Book(bookId, "Go");
        var publisher = new Publisher(publisherId, "O’Reilly Media");
        using (var uow = _unitOfWorkManager.Begin(new UnitOfWorkOptions { IsTransactional = true }))
        {
            await _bookRepository.AddAsync(book);
            await _publisherRepository.AddAsync(publisher);
            await uow.CommitChangesAsync();
        }

        var retrievalBook = await _bookRepository.GetByIdAsync(bookId);
        var retrievalPublisher = await _publisherRepository.GetByIdAsync(publisherId);
        Assert.NotNull(retrievalBook);
        Assert.NotNull(retrievalPublisher);
    }
}