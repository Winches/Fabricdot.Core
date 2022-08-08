using Fabricdot.Core.Delegates;

namespace Fabricdot.Core.Tests.Delegates;

public class NullDisposableTests : TestBase
{
    [Fact]
    public void Dispose_Should_Correctly()
    {
        NullDisposable.Instance.Dispose();
        NullDisposable.Instance.Dispose();
    }
}