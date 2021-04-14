
namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data
{
    public class FakeEntityChangeTracker : EfEntityChangeTracker
    {
        /// <inheritdoc />
        public FakeEntityChangeTracker(FakeDbContext context) : base(context)
        {
        }
    }
}