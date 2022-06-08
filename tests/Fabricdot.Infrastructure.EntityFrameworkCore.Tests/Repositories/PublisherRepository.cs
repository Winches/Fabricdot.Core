using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Repositories;

internal class PublisherRepository : EfRepository<FakeSecondDbContext, Publisher, string>, IPublisherRepository
{
    /// <inheritdoc />
    public PublisherRepository(IDbContextProvider<FakeSecondDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}