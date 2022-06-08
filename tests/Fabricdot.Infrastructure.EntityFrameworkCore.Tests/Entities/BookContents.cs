using Fabricdot.Domain.Entities;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities;

public class BookContents : FullAuditEntity<int>
{
    public string BookId { get; private set; }

    public string Introduction { get; private set; }

    public BookContents(string introduction)
    {
        Introduction = introduction;
    }

    private BookContents()
    {
    }
}