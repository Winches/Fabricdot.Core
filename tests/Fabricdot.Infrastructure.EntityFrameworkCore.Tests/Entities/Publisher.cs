using Ardalis.GuardClauses;
using Fabricdot.Domain.Entities;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities;

public class Publisher : FullAuditAggregateRoot<string>
{
    public string Name { get; private set; }

    public Publisher(string id, string name)
    {
        Guard.Against.NullOrEmpty(id, nameof(id));
        Guard.Against.NullOrEmpty(name, nameof(name));
        Name = name;
        Id = id;
    }

    private Publisher()
    {
    }
}