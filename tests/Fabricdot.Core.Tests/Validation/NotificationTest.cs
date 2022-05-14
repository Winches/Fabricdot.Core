using System.Linq;
using Fabricdot.Core.Validation;
using FluentAssertions;
using Xunit;

namespace Fabricdot.Core.Tests.Validation
{
    public class NotificationTest
    {
        [Fact]
        public void Add_GivenNewKey_Correctly()
        {
            var notification = new Notification();
            const string key = "SomeKey";
            var error = new Notification.Error("error1");
            notification.Add(key, error);
            var actual = notification.Errors.First();

            actual.Key.Should().Be(key);
            actual.Value.Should().Contain(error);
        }

        [Fact]
        public void Add_GivenExistedKey_Correctly()
        {
            var notification = new Notification();
            const string key1 = "key1";
            var error1 = new Notification.Error("error1");
            var error2 = new Notification.Error("error2");
            notification.Add(key1, error1);
            notification.Add(key1, error2);
            var actual = notification.Errors.First();

            actual.Value.Should().Contain(error1);
            actual.Value.Should().Contain(error2);
        }

        [Fact]
        public void IsValid_ReturnCorrectly()
        {
            var notification = new Notification();

            notification.IsValid.Should().BeTrue();
            notification.Add("key1", new Notification.Error("error1"));
            notification.IsValid.Should().BeFalse();
        }
    }
}