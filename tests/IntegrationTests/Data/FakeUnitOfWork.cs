using System;
using Fabricdot.Infrastructure.EntityFrameworkCore;

namespace IntegrationTests.Data
{
    public class FakeUnitOfWork : UnitOfWorkBase
    {
        /// <inheritdoc />
        public FakeUnitOfWork(FakeDbContext context, IServiceProvider serviceProvider) : base(context, serviceProvider)
        {
        }
    }
}