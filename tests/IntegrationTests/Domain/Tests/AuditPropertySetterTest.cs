using Fabricdot.Common.Core.Security;
using Fabricdot.Infrastructure.Core.Domain.Auditing;
using IntegrationTests.Data.Entities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace IntegrationTests.Domain.Tests
{
    public class AuditPropertySetterTest : TestBase
    {
        private readonly IAuditPropertySetter _auditPropertySetter;
        private readonly ICurrentUser _currentUser;

        public AuditPropertySetterTest()
        {
            var provider = ServiceScope.ServiceProvider;
            _auditPropertySetter = provider.GetRequiredService<IAuditPropertySetter>();
            _currentUser = provider.GetRequiredService<ICurrentUser>();
        }

        [Fact]
        public void TestSetCreationProperties()
        {
            var entity = new Book("1", "Effective C#");
            _auditPropertySetter.SetCreationProperties(entity);

            Assert.False(string.IsNullOrWhiteSpace(entity.CreatorId));
            Assert.Equal(entity.CreatorId, _currentUser.Id);
            Assert.NotEqual(default, entity.CreationTime);

            //value can not be overwrite
            entity.ChangeCreatorId("2");
            var creatorId = entity.CreatorId;
            var creationTime = entity.CreationTime;
            _auditPropertySetter.SetCreationProperties(entity);

            Assert.Equal(entity.CreatorId, creatorId);
            Assert.Equal(entity.CreationTime, creationTime);
        }

        [Fact]
        public void TestSetModificationProperties()
        {
            var entity = new Book("1", "Effective C#");
            _auditPropertySetter.SetModificationProperties(entity);

            Assert.False(string.IsNullOrWhiteSpace(entity.LastModifierId));
            Assert.Equal(entity.LastModifierId, _currentUser.Id);
            Assert.NotEqual(default, entity.LastModificationTime);

            var date = entity.LastModificationTime;
            _auditPropertySetter.SetModificationProperties(entity);

            Assert.Equal(entity.LastModifierId, _currentUser.Id);
            Assert.True(entity.LastModificationTime > date);
        }

        [Fact]
        public void TestSetDeletionProperties()
        {
            var entity = new Book("1", "Effective C#");
            _auditPropertySetter.SetDeletionProperties(entity);

            Assert.False(string.IsNullOrWhiteSpace(entity.DeleterId));
            Assert.Equal(entity.DeleterId, _currentUser.Id);
            Assert.NotEqual(default, entity.DeletionTime);
            Assert.True(entity.IsDeleted);

            var date = entity.DeletionTime;
            var userId = entity.DeleterId;
            _auditPropertySetter.SetDeletionProperties(entity);

            Assert.Equal(date, entity.DeletionTime);
            Assert.Equal(userId, entity.DeleterId);
        }
    }
}