using System;
using System.Linq;
using Fabricdot.Domain.Entities;
using Fabricdot.Domain.Events;
using Xunit;

namespace Fabricdot.Domain.Tests;

public class AggregateRootTest
{
    public class Book : AggregateRoot<Guid>
    {
    }

    [Fact]
    public void New_GenerateConcurrencyStamp()
    {
        var book = new Book();
        var actual = book.ConcurrencyStamp;
        Assert.NotEmpty(actual);
    }

    [Fact]
    public void AddDomainEvent_GivenEvent_Correctly()
    {
        var book = new Book();
        var expected = new EntityCreatedEvent<Book>(book);
        book.AddDomainEvent(expected);
        var actual = book.DomainEvents.Single();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void AddDomainEvent_GivenNull_ThrownException()
    {
        var book = new Book();
        void Action() => book.AddDomainEvent(null);
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void RemoveDomainEvent_GivenEvent_Correctly()
    {
        var book = new Book();
        var expected = new EntityCreatedEvent<Book>(book);
        book.AddDomainEvent(expected);
        book.RemoveDomainEvent(expected);

        Assert.DoesNotContain(expected, book.DomainEvents);
    }

    [Fact]
    public void ClearDomainEvents_EmptyEvents()
    {
        var book = new Book();
        var domainEvent = new EntityCreatedEvent<Book>(book);
        book.AddDomainEvent(domainEvent);
        book.ClearDomainEvents();

        Assert.Empty(book.DomainEvents);
    }
}