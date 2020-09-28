using System;
using Fabricdot.Domain.Core.Services;
using Fabricdot.Infrastructure.EntityFrameworkCore;

namespace IntegrationTests.Data
{
    public class FakeRepository : EfRepository<FakeEntity, string>, IFakeRepository
    {
        /// <inheritdoc />
        public FakeRepository(FakeDbContext context, IServiceProvider serviceProvider) : base(context, serviceProvider)
        {
        }
    }

    public interface IFakeRepository : IRepository<FakeEntity, string>
    {
    }
}