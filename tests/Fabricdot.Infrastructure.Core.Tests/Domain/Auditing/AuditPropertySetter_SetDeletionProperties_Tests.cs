using System.Diagnostics.CodeAnalysis;
using Fabricdot.Domain.Core.SharedKernel;
using Xunit;

namespace Fabricdot.Infrastructure.Core.Tests.Domain.Auditing
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class AuditPropertySetter_SetDeletionProperties_Tests : AuditPropertySetterTestBase
    {
        [Fact]
        public void SetDeletionProperties_GivenIHasDeletionTime_SetDeletionTime()
        {
            var targetObject = new FakeAuditObject();
            var startTime = SystemClock.Now;
            AuditPropertySetter.SetDeletionProperties(targetObject);
            var endTime = SystemClock.Now;
            var actual = targetObject.DeletionTime;
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, startTime, endTime);
        }

        [Fact]
        public void SetDeletionProperties_GivenIHasDeletionTimeWithValue_DoNothing()
        {
            var targetObject = GetAuditedObject();
            var expected = targetObject.DeletionTime;
            AuditPropertySetter.SetDeletionProperties(targetObject);
            var actual = targetObject.DeletionTime;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetDeletionProperties_GivenIHasDeleterId_SetDeleterId()
        {
            var targetObject = new FakeAuditObject();
            var expected = CurrentUser.Id;
            AuditPropertySetter.SetDeletionProperties(targetObject);
            var actual = targetObject.DeleterId;
            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(GetAuditObjects))]
        public void SetDeletionProperties_GivenISoftDeleted_IsDeletedBeTrue(FakeAuditObject targetObject)
        {
            AuditPropertySetter.SetDeletionProperties(targetObject);
            var condition = targetObject.IsDeleted;
            Assert.True(condition);
        }

        [Theory]
        [MemberData(nameof(GetNonAuditObjects))]
        public void SetDeletionProperties_GivenNonAuditedObject_DoNothing(object targetObject)
        {
            AuditPropertySetter.SetDeletionProperties(targetObject);
        }
    }
}