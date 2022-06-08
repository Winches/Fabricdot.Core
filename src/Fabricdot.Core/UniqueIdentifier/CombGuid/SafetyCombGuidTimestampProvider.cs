using System;
using System.Threading;

namespace Fabricdot.Core.UniqueIdentifier.CombGuid;

public sealed class SafetyCombGuidTimestampProvider : ITimestampProvider
{
    public static readonly SafetyCombGuidTimestampProvider Instance = new();
    private long _counter = DateTime.UtcNow.Ticks / 10000L;

    private SafetyCombGuidTimestampProvider()
    {
    }

    public long GetTimestamp() => Interlocked.Increment(ref _counter);
}