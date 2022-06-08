using Ardalis.Specification;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities;

public sealed class AuthorFilterSpecification : Specification<Author>
{
    public AuthorFilterSpecification(int authorId)
    {
        Query.Where(v => v.Id == authorId);
    }
}