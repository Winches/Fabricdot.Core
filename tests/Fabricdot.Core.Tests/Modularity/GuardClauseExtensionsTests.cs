using Ardalis.GuardClauses;
using Fabricdot.Core.Modularity;

namespace Fabricdot.Core.Tests.Modularity;

public class GuardClauseExtensionsTests : TestBase
{
    [Fact]
    public void InvalidModuleType_WhenGuardIsNull_ThrowException()
    {
        Invoking(() => ((IGuardClause)null).InvalidModuleType(GetType(), Create<string>()))
                     .Should()
                     .Throw<ArgumentException>();
    }

    [Fact]
    public void InvalidModuleType_WhenTypeIsNull_ThrowException()
    {
        Invoking(() => Guard.Against.InvalidModuleType(null, Create<string>()))
                     .Should()
                     .Throw<ArgumentNullException>();
    }

    [Fact]
    public void InvalidModuleType_WhenTypeIsInvalid_ThrowException()
    {
        Invoking(() => Guard.Against.InvalidModuleType(GetType(), Create<string>()))
                     .Should()
                     .Throw<ArgumentException>();
    }
}