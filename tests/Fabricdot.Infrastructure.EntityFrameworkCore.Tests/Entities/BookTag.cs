using Fabricdot.Domain.Entities;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities
{
    public class BookTag : FullAuditEntity<int>
    {
        public string Name { get; private set; }

        internal BookTag(string name)
        {
            Name = name;
        }

        protected BookTag()
        {
        }

        public void MarkDeleted() => IsDeleted = true;
    }
}