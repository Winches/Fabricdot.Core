using System.Linq;
using Fabricdot.Core.Validation;
using Xunit;

namespace Fabricdot.Core.Tests
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
            Assert.Equal(key, actual.Key);
            Assert.Contains(error, actual.Value);
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
            Assert.Contains(error1, actual.Value);
            Assert.Contains(error2, actual.Value);
        }
    }
}