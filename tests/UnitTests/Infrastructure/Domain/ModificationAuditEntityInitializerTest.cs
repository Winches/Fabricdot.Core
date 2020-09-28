using Fabricdot.Domain.Core.Auditing;
using Fabricdot.Infrastructure.Core.Domain.Auditing;
using Xunit;

namespace UnitTests.Infrastructure.Domain
{
    public class ModificationAuditEntityInitializerTest
    {
        public class TestEntity : AuditEntity<int>
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
            ModificationAuditEntityInitializer.Init(entity, userId);
            Assert.Equal(entity.LastModifierId, userId);
            Assert.NotEqual(default, entity.LastModificationTime);
        }
    }
}