using Fabricdot.Infrastructure.EntityFrameworkCore;

namespace IntegrationTests.Data
{
    public class FakeEntityChangeTracker : EfEntityChangeTracker
    {
        /// <inheritdoc />
        public FakeEntityChangeTracker(FakeDbContext context) : base(context)
        {
        }
    }
}