using Fabricdot.Core.Validation;

namespace Fabricdot.Core.Tests.Validation;

public class NotificationTests : TestFor<Notification>
{
    [AutoData]
    [Theory]
    public void Add_GivenNewKey_Correctly(
        string key,
        Notification.Error error)
    {
        Sut.Add(key, error);
        var actual = Sut.Errors.First();

        actual.Key.Should().Be(key);
        actual.Value.Should().Contain(error);
    }

    [AutoData]
    [Theory]
    public void Add_GivenExistedKey_Correctly(
        string key,
        Notification.Error error1,
        Notification.Error error2)
    {
        Sut.Add(key, error1);
        Sut.Add(key, error2);
        var actual = Sut.Errors.First();

        actual.Value.Should().Contain(error1);
        actual.Value.Should().Contain(error2);
    }

    [AutoData]
    [Theory]
    public void IsValid_ReturnCorrectly(
        string key,
        Notification.Error error)
    {
        Sut.IsValid.Should().BeTrue();
        Sut.Add(key, error);

        Sut.IsValid.Should().BeFalse();
    }
}
