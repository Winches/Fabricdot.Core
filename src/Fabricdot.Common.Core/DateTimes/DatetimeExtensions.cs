using System;

namespace Fabricdot.Common.Core.DateTimes
{
    public static class DateTimeExtensions
    {
        public static DateTime ToDateTime(this double timestamp)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddMilliseconds(timestamp).ToLocalTime();
            return dateTime;
        }

        public static double ToTimestamp(this DateTime dateTime)
        {
            return dateTime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
        }
    }
}