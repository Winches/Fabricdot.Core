using Fabricdot.Core.UniqueIdentifier;
using Fabricdot.Core.UniqueIdentifier.CombGuid;

namespace Fabricdot.Core.Tests.UniqueIdentifier;

public class CombGuidGeneratorTests : SafetyCombGuidGeneratorTests
{
    protected override CombGuidGenerator CreateSut() => GuidFactories.Comb;
}