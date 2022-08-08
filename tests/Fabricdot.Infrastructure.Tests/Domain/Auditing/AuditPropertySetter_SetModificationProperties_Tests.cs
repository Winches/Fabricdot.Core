using Fabricdot.Domain.Entities;
using Fabricdot.Domain.SharedKernel;

namespace Fabricdot.Infrastructure.Tests.Domain.Auditing;

public class AuditPropertySetter_SetModificationProperties_Tests : AuditPropertySetterTestBase
{
    [Fact]
    public void SetModificationProperties_GivenIHasModificationTime_SetLastModificationTime()
    {
        var targetObject = Create<FullAuditEntity<int>>();
        var low = SystemClock.Now;
        Sut.SetModificationProperties(targetObject);
        var high = SystemClock.Now;

        targetObject.LastModificationTime.Should()
                                         .BeOnOrAfter(low).And
                                         .BeOnOrBefore(high);
    }

    [Fact]
    public void SetModificationProperties_GivenIHasModifierId_SetLastModifierId()
    {
        var targetObject = Create<FullAuditEntity<int>>();
        var expected = CurrentUser.Id;
        Sut.SetModificationProperties(targetObject);

        targetObject.LastModifierId.Should().Be(expected);
    }

    [Theory]
    [InlineAutoData(null)]
    [InlineAutoData]
    public void SetModificationProperties_GivenNonAuditedObject_DoNothing(object targetObject)
    {
        Sut.SetModificationProperties(targetObject);
    }
}