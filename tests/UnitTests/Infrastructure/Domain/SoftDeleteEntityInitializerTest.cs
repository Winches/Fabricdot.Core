using Fabricdot.Domain.Core.Auditing;
using Fabricdot.Infrastructure.Core.Domain.Auditing;
using Xunit;

namespace UnitTests.Infrastructure.Domain
{
    public class SoftDeleteEntityInitializerTest
    {
        public class TestEntity : ISoftDelete
        {
            /// <inheritdoc />
            public bool IsDeleted { get; private set; }
        }

        [Fact]
        public void Test()
        {
            var entity = new TestEntity();
            SoftDeleteEntityInitializer.Init(entity);
            Assert.True(entity.IsDeleted);
        }
    }
}