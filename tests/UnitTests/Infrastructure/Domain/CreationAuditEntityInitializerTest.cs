using Fabricdot.Domain.Core.Auditing;
using Fabricdot.Infrastructure.Core.Domain.Auditing;
using Xunit;

namespace UnitTests.Infrastructure.Domain
{
    public class CreationAuditEntityInitializerTest
    {
        public class TestEntity : CreationAuditEntity<int>
        {
            public TestEntity(int id)
            {
                Id = id;
            }
        }

        [Fact]
        public void Test()
        {
            const string userId = "UserId";
            var entity = new TestEntity(1);
            CreationAuditEntityInitializer.Init(entity, userId);
            Assert.Equal(entity.CreatorId, userId);
            Assert.NotEqual(default, entity.CreationTime);
        }
    }
}