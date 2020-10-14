using System;
using Fabricdot.Domain.Core.SharedKernel;
using Xunit;

namespace UnitTests.Domain.SharedKernel
{
    public class SystemClockTest
    {
        [Fact]
        public void TestUtcTime()
        {
            SystemClock.Configure(DateTimeKind.Utc);
            Assert.Equal(DateTimeKind.Utc, SystemClock.Now.Kind);
        }

        [Fact]
        public void TestLocalTime()
        {
            SystemClock.Configure(DateTimeKind.Local);
            Assert.Equal(DateTimeKind.Local, SystemClock.Now.Kind);
        }

        [Fact]
        public void TestNormalize()
        {
            SystemClock.Configure(DateTimeKind.Utc);
            var localTime = DateTime.Now;
            var utcTime = localTime.ToUniversalTime();
            Assert.Equal(utcTime, SystemClock.Normalize(localTime));
            Assert.Equal(utcTime, SystemClock.Normalize(utcTime));

            SystemClock.Configure(DateTimeKind.Local);
            Assert.Equal(localTime, SystemClock.Normalize(localTime));
            Assert.Equal(localTime, SystemClock.Normalize(utcTime));
        }
    }
}
