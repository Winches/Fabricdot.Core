using Ardalis.GuardClauses;
using Fabricdot.Domain.Core.Entities;
using Fabricdot.Infrastructure.EntityFrameworkCore.Configurations;

namespace IntegrationTests.Data.Entities
{
    public class Book : FullAuditAggregateRoot<string>
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
    }

    public class BookConfiguration : EntityTypeConfigurationBase<Book>
    {
    }
}