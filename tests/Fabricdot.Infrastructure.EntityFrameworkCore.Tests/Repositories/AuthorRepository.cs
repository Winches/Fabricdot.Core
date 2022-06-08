using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Repositories;

internal class AuthorRepository : EfRepository<FakeDbContext, Author, int>, IAuthorRepository
{
    /// <inheritdoc />
    public AuthorRepository(IDbContextProvider<FakeDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}