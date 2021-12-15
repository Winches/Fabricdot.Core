using System;
using Fabricdot.Domain.SharedKernel;
using Xunit;

namespace Fabricdot.Domain.Tests
{
    public class SystemClockTest
    {
        [Fact]
        public void Now_ConfigureUtc_ReturnUtcTime()
        {
            SystemClock.Configure(DateTimeKind.Utc);
            Assert.Equal(DateTimeKind.Utc, SystemClock.Now.Kind);
        }

        [Fact]
        public void Now_ConfigureLocal_ReturnLocalTime()
        {
            SystemClock.Configure(DateTimeKind.Local);
            Assert.Equal(DateTimeKind.Local, SystemClock.Now.Kind);
        }

        [Fact]
        public void Normalize_ConfigureUtcGivenLocalTime_ReturnUtcTime()
        {
            const DateTimeKind expected = DateTimeKind.Utc;
            SystemClock.Configure(expected);
            var dateTime = SystemClock.Normalize(DateTime.Now);
            Assert.Equal(expected, dateTime.Kind);
        }

        [Fact]
        public void Normalize_ConfigureUtcGivenUnspecifiedTime_ReturnUtcTime()
        {
            const DateTimeKind expected = DateTimeKind.Utc;
            SystemClock.Configure(expected);
            var dateTime = SystemClock.Normalize(new DateTime());
            Assert.Equal(expected, dateTime.Kind);
        }

        [Fact]
        public void Normalize_ConfigureLocalGivenUtcTime_ReturnLocalTime()
        {
            const DateTimeKind expected = DateTimeKind.Local;
            SystemClock.Configure(expected);
            var dateTime = SystemClock.Normalize(DateTime.UtcNow);
            Assert.Equal(expected, dateTime.Kind);
        }

        [Fact]
        public void Normalize_ConfigureLocalGivenUnspecifiedTime_ReturnLocalTime()
        {
            const DateTimeKind expected = DateTimeKind.Local;
            SystemClock.Configure(expected);
            var dateTime = SystemClock.Normalize(new DateTime());
            Assert.Equal(expected, dateTime.Kind);
        }

        [Fact]
        public void Normalize_ConfigureUnspecifiedGivenDateTime_ReturnGivenTime()
        {
            SystemClock.Configure(DateTimeKind.Unspecified);
            var utcTime = SystemClock.Normalize(DateTime.UtcNow);
            var localTime = SystemClock.Normalize(DateTime.Now);

            Assert.Equal(DateTimeKind.Utc, utcTime.Kind);
            Assert.Equal(DateTimeKind.Local, localTime.Kind);
        }
    }
}