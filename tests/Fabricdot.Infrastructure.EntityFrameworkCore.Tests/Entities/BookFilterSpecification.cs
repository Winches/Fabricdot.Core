using Ardalis.Specification;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities
{
    public sealed class BookFilterSpecification : Specification<Book>
    {
        public BookFilterSpecification(string name)
        {
            Query.Where(v => v.Name == name);
        }
    }
}