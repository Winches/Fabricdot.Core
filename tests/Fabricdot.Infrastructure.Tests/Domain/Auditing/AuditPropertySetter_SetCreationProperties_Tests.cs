using Fabricdot.Domain.Entities;
using Fabricdot.Domain.SharedKernel;

namespace Fabricdot.Infrastructure.Tests.Domain.Auditing;

public class AuditPropertySetter_SetCreationProperties_Tests : AuditPropertySetterTestBase
{
    [Fact]
    public void SetCreationProperties_GivenIHasCreationTime_SetCreationTime()
    {
        var targetObject = Create<FullAuditEntity<int>>();
        var startTime = SystemClock.Now;
        Sut.SetCreationProperties(targetObject);
        var endTime = SystemClock.Now;

        targetObject.CreationTime.Should().BeOnOrAfter(startTime).And.BeOnOrBefore(endTime);
    }

    [Fact]
    public void SetCreationProperties_GivenIHasCreationTimeWithValue_DoNothing()
    {
        var expected = Create<DateTime>();
        var mock = Mock<FullAuditEntity<int>>();
        var targetObject = mock.Object;
        mock.SetupProperty(v => v.CreationTime, expected);
        Sut.SetCreationProperties(targetObject);

        targetObject.CreationTime.Should().Be(expected);
    }

    [Fact]
    public void SetCreationProperties_GivenIHasCreatorId_SetCreatorId()
    {
        var targetObject = Create<FullAuditEntity<int>>();
        var expected = CurrentUser.Id;
        Sut.SetCreationProperties(targetObject);

        targetObject.CreatorId.Should().Be(expected);
    }

    [Fact]
    public void SetCreationProperties_GivenIHasCreatorIdWithValue_DoNothing()
    {
        var expected = Create<string>();
        var mock = Mock<FullAuditEntity<int>>();
        var targetObject = mock.Object;
        mock.SetupProperty(v => v.CreatorId, expected);
        Sut.SetCreationProperties(targetObject);

        targetObject.CreatorId.Should().Be(expected);
    }

    [Theory]
    [InlineAutoData(null)]
    [InlineAutoData]
    public void SetCreationProperties_GivenNonAuditedObject_DoNothing(object targetObject)
    {
        Sut.SetCreationProperties(targetObject);
    }
}
