using Ardalis.Specification;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities;

public sealed class BookFilterSpecification : Specification<Book>
{
    public BookFilterSpecification(string name)
    {
        Query.Where(v => v.Name == name);
    }

    public BookFilterSpecification(
        string bookId,
        bool includeDetails)
    {
        Query.Where(v => v.Id == bookId);
        if (includeDetails)
            Query.Include(v => v.Tags)
                 .Include(v => v.Contents);
    }
}