using Fabricdot.Domain.Entities;
using Fabricdot.Domain.SharedKernel;

namespace Fabricdot.Infrastructure.Tests.Domain.Auditing;

public class AuditPropertySetter_SetDeletionProperties_Tests : AuditPropertySetterTestBase
{
    [Fact]
    public void SetDeletionProperties_GivenIHasDeletionTime_SetDeletionTime()
    {
        var targetObject = Create<FullAuditEntity<int>>();
        var startTime = SystemClock.Now;
        Sut.SetDeletionProperties(targetObject);
        var endTime = SystemClock.Now;

        targetObject.DeletionTime.Should().BeOnOrAfter(startTime).And.BeOnOrBefore(endTime);
    }

    [Fact]
    public void SetDeletionProperties_GivenIHasDeletionTimeWithValue_DoNothing()
    {
        var expected = Create<DateTime>();
        var mock = Mock<FullAuditEntity<int>>();
        var targetObject = mock.Object;
        mock.SetupProperty(v => v.DeletionTime, expected);
        Sut.SetDeletionProperties(targetObject);

        targetObject.DeletionTime.Should().Be(expected);
    }

    [Fact]
    public void SetDeletionProperties_GivenIHasDeleterId_SetDeleterId()
    {
        var targetObject = Create<FullAuditEntity<int>>();
        var expected = CurrentUser.Id;
        Sut.SetDeletionProperties(targetObject);

        targetObject.DeleterId.Should().Be(expected);
    }

    [Fact]
    public void SetDeletionProperties_GivenISoftDeleted_IsDeletedBeTrue()
    {
        var targetObject = Create<FullAuditEntity<int>>();
        Sut.SetDeletionProperties(targetObject);

        targetObject.IsDeleted.Should().BeTrue();
    }

    [Theory]
    [InlineAutoData(null)]
    [InlineAutoData]
    public void SetDeletionProperties_GivenNonAuditedObject_DoNothing(object targetObject)
    {
        Sut.SetDeletionProperties(targetObject);
    }
}
