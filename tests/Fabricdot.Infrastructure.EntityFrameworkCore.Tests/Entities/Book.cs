using Ardalis.GuardClauses;
using Fabricdot.Domain.Core.Entities;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities
{
    public class Book : AuditAggregateRoot<string>
    {
        public string Name { get; private set; }

        public Book(string id, string name)
        {
            Name = name;
            Id = id;
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
    }
}