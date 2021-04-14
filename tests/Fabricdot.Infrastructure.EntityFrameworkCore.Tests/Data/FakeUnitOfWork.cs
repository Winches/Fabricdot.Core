using System;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data
{
    public class FakeUnitOfWork : UnitOfWorkBase
    {
        /// <inheritdoc />
        public FakeUnitOfWork(FakeDbContext context, IServiceProvider serviceProvider) : base(context, serviceProvider)
        {
        }
    }
}