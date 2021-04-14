using Fabricdot.Domain.Core.Entities;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities
{
    public class Author : FullAuditAggregateRoot<int>
    {
        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public Author(int id, string firstName, string lastName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }

        public void MarkDeleted() => IsDeleted = true;
    }
}