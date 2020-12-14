using Ardalis.Specification;
using IntegrationTests.Data.Entities;

namespace IntegrationTests.Data.Repositories
{
    public sealed class BookSpecification : Specification<Book>
    {
        public BookSpecification(string name)
        {
            Query.Where(v => v.Name == name);
        }
    }
}