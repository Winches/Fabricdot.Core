using System.Diagnostics.CodeAnalysis;
using Fabricdot.Domain.Core.SharedKernel;
using Xunit;

namespace Fabricdot.Infrastructure.Core.Tests.Domain.Auditing
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class AuditPropertySetter_SetCreationProperties_Tests : AuditPropertySetterTestBase
    {
        [Fact]
        public void SetCreationProperties_GivenIHasCreationTime_SetCreationTime()
        {
            var targetObject = new FakeAuditObject();
            var startTime = SystemClock.Now;
            AuditPropertySetter.SetCreationProperties(targetObject);
            var endTime = SystemClock.Now;
            var actual = targetObject.CreationTime;
            Assert.NotEqual(default, actual);
            Assert.InRange(actual, startTime, endTime);
        }

        [Fact]
        public void SetCreationProperties_GivenIHasCreationTimeWithValue_DoNothing()
        {
            var targetObject = GetAuditedObject();
            var expected = targetObject.CreationTime;
            AuditPropertySetter.SetCreationProperties(targetObject);
            var actual = targetObject.CreationTime;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetCreationProperties_GivenIHasCreatorId_SetCreatorId()
        {
            var targetObject = new FakeAuditObject();
            var expected = CurrentUser.Id;
            AuditPropertySetter.SetCreationProperties(targetObject);
            var actual = targetObject.CreatorId;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetCreationProperties_GivenIHasCreatorIdWithValue_DoNothing()
        {
            var targetObject = GetAuditedObject();
            var expected = targetObject.CreatorId;
            AuditPropertySetter.SetCreationProperties(targetObject);
            var actual = targetObject.CreatorId;
            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(GetNonAuditObjects))]
        public void SetCreationProperties_GivenNonAuditedObject_DoNothing(object targetObject)
        {
            AuditPropertySetter.SetCreationProperties(targetObject);
        }
    }
}