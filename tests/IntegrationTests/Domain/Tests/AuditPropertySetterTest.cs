using System;
using Fabricdot.Common.Core.Security;
using Fabricdot.Domain.Core.Auditing;
using Fabricdot.Domain.Core.SharedKernel;
using Fabricdot.Infrastructure.Core.Domain.Auditing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace IntegrationTests.Domain.Tests
{
    public class AuditPropertySetterTest
    {
        private readonly IAuditPropertySetter _auditPropertySetter;
        private readonly ICurrentUser _currentUser;

        public class TestEntity : IHasCreationTime, IHasCreatorId, IHasModificationTime, IHasModifierId, IHasDeleterId,
            IHasDeletionTime
        {
            public TestEntity()
            {
            }

            public TestEntity(
                DateTime creationTime,
                string creationId,
                DateTime lastModificationTime,
                string lastModifierId,
                string deleterId,
                DateTime? deletionTime)
            {
                CreationTime = creationTime;
                CreatorId = creationId;
                LastModificationTime = lastModificationTime;
                LastModifierId = lastModifierId;
                DeleterId = deleterId;
                DeletionTime = deletionTime;
            }

            /// <inheritdoc />
            public DateTime CreationTime { get; private set; }

            /// <inheritdoc />
            public string CreatorId { get; private set; }

            /// <inheritdoc />
            public DateTime LastModificationTime { get; private set; }

            /// <inheritdoc />
            public string LastModifierId { get; private set; }

            /// <inheritdoc />
            public bool IsDeleted { get; private set; } = false;

            /// <inheritdoc />
            public string DeleterId { get; private set; }

            /// <inheritdoc />
            public DateTime? DeletionTime { get; private set; }
        }

        public AuditPropertySetterTest()
        {
            var provider = ContainerBuilder.GetServiceProvider();
            _auditPropertySetter = provider.GetRequiredService<IAuditPropertySetter>();
            _currentUser = provider.GetRequiredService<ICurrentUser>();
        }

        [Fact]
        public void TestSetCreationProperties()
        {
            var entity = new TestEntity();
            _auditPropertySetter.SetCreationProperties(entity);

            Assert.False(string.IsNullOrWhiteSpace(entity.CreatorId));
            Assert.Equal(entity.CreatorId, _currentUser.Id);
            Assert.NotEqual(default, entity.CreationTime);

            var date = SystemClock.Now;
            const string userId = "1";
            entity = new TestEntity(date, userId, default, default, default, default);
            _auditPropertySetter.SetCreationProperties(entity);

            Assert.Equal(entity.CreatorId, userId);
            Assert.Equal(entity.CreationTime, date);
        }

        [Fact]
        public void TestSetModificationProperties()
        {
            var entity = new TestEntity();
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
            var entity = new TestEntity();
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