using System;
using Fabricdot.Core.UniqueIdentifier.CombGuid;

namespace Fabricdot.Core.UniqueIdentifier;

public sealed class DefaultCombGuidTimestampProvider : ITimestampProvider
{
    public static readonly DefaultCombGuidTimestampProvider Instance = new();

    private DefaultCombGuidTimestampProvider()
    {
    }

    public long GetTimestamp() => DateTime.UtcNow.Ticks / 10000L;
}