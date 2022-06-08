using System;
using System.Collections.Generic;
using System.Linq;
using Ardalis.GuardClauses;
using Fabricdot.Domain.Entities;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities;

public class Book : FullAuditAggregateRoot<string>
{
    private readonly List<BookTag> _tags = new List<BookTag>();
    public IReadOnlyCollection<BookTag> Tags => _tags.AsReadOnly();
    public string Name { get; private set; }
    public BookContents Contents { get; internal set; }

    public Book(
        string id,
        string name)
    {
        Name = name;
        Id = id;
    }

    public Book(
        string id,
        string name,
        ICollection<string> tags)
    {
        Name = name;
        Id = id;
        tags?.ForEach(v => AddTag(v));
    }

    public void ChangeName(string name)
    {
        Guard.Against.Null(name, nameof(name));
        Name = name;
    }

    public void ChangeCreatorId(string creatorId)
    {
        CreatorId = creatorId;
    }

    public void AddTag(
        string name,
        bool isDeleted = false)
    {
        var isExisted = _tags.Any(v => v.Name == name);
        if (isExisted)
            throw new ArgumentException("Tag already existed.");
        var bookTag = new BookTag(name);
        if (isDeleted)
            bookTag.MarkDeleted();
        _tags.Add(bookTag);
    }

    public void RemoveTag(string name)
    {
        var tag = _tags.SingleOrDefault(v => v.Name == name);
        if (!_tags.Remove(tag))
            throw new ArgumentException("Tag not existed.");
    }
}