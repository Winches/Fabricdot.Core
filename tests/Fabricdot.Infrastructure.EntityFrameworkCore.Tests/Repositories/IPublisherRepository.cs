using Fabricdot.Domain.Core.Services;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Repositories
{
    public interface IPublisherRepository : IRepository<Publisher, string>
    {
    }
}