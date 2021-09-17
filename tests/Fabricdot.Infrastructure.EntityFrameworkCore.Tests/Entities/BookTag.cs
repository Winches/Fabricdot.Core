using Fabricdot.Domain.Core.Entities;

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
    }
}