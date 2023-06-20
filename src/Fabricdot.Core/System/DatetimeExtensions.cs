// ReSharper disable once CheckNamespace

using Ardalis.GuardClauses;

namespace System;

public static class DateTimeExtensions
{
    private static readonly DateTime s_timestampStartTime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

    public static DateTime ToDateTime(this double timestamp)
    {
        Guard.Against.Negative(timestamp, nameof(timestamp));

        return s_timestampStartTime.AddMilliseconds(timestamp).ToLocalTime();
    }

    public static long ToTimestamp(this DateTime dateTime)
    {
        return (long)dateTime.ToUniversalTime().Subtract(s_timestampStartTime).TotalMilliseconds;
    }
}
