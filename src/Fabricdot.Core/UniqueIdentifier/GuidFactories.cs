using Fabricdot.Core.UniqueIdentifier.CombGuid;

namespace Fabricdot.Core.UniqueIdentifier;

public static class GuidFactories
{
    private static readonly Lazy<CombGuidGenerator> s_lazyComb = new(() => new CombGuidGenerator(DefaultCombGuidTimestampProvider.Instance));

    private static readonly Lazy<CombGuidGenerator> s_lazySafetyComb = new(() => new CombGuidGenerator(SafetyCombGuidTimestampProvider.Instance));

    public static CombGuidGenerator Comb => s_lazyComb.Value;

    public static CombGuidGenerator SafetyComb => s_lazySafetyComb.Value;
}
