using System.Diagnostics.CodeAnalysis;
using Fabricdot.Domain.SharedKernel;
using FluentAssertions;
using Xunit;

namespace Fabricdot.Infrastructure.Tests.Domain.Auditing;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class AuditPropertySetter_SetModificationProperties_Tests : AuditPropertySetterTestBase
{
    [Theory]
    [MemberData(nameof(GetAuditObjects))]
    public void SetModificationProperties_GivenIHasModificationTime_SetLastModificationTime(
        FakeAuditObject targetObject)
    {
        var low = SystemClock.Now;
        AuditPropertySetter.SetModificationProperties(targetObject);
        var high = SystemClock.Now;

        targetObject.LastModificationTime.Should()
                                         .BeOnOrAfter(low).And
                                         .BeOnOrBefore(high);
    }

    [Theory]
    [MemberData(nameof(GetAuditObjects))]
    public void SetModificationProperties_GivenIHasModifierId_SetLastModifierId(FakeAuditObject targetObject)
    {
        var expected = CurrentUser.Id;
        AuditPropertySetter.SetModificationProperties(targetObject);
        var actual = targetObject.LastModifierId;
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetNonAuditObjects))]
    public void SetModificationProperties_GivenNonAuditedObject_DoNothing(object targetObject)
    {
        AuditPropertySetter.SetModificationProperties(targetObject);
    }
}