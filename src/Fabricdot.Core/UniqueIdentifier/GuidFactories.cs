using System;
using Fabricdot.Core.UniqueIdentifier.CombGuid;

namespace Fabricdot.Core.UniqueIdentifier;

public static class GuidFactories
{
    private static readonly Lazy<CombGuidGenerator> _lazyComb = new(() => new CombGuidGenerator(DefaultCombGuidTimestampProvider.Instance));

    private static readonly Lazy<CombGuidGenerator> _lazySafetyComb = new(() => new CombGuidGenerator(SafetyCombGuidTimestampProvider.Instance));

    public static CombGuidGenerator Comb => _lazyComb.Value;

    public static CombGuidGenerator SafetyComb => _lazySafetyComb.Value;
}