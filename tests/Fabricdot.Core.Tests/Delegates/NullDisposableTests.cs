using Fabricdot.Core.Delegates;
using Xunit;

namespace Fabricdot.Core.Tests.Delegates
{
    public class NullDisposableTests
    {
        [Fact]
        public void Dispose_DoNothing()
        {
            NullDisposable.Instance.Dispose();
        }
    }
}