using Fabricdot.Core.Delegates;

namespace Fabricdot.Core.Tests.Delegates;

public class DisposeActionTests : TestFor<Mock<Action>>
{
    [Fact]
    public void Constructor_GivenNull_TrowException()
    {
        var sut = typeof(DisposeAction).GetConstructors();

        Create<GuardClauseAssertion>().Verify(sut);
    }

    [Fact]
    public void Dispose_GivenAction_InvokeAction()
    {
        using (var scope = new DisposeAction(Sut.Object))
        {
            Sut.Verify(v => v(), Times.Never);
        }
        Sut.Verify(v => v(), Times.Once);
    }

    [Fact]
    public void Dispose_InvokeTwice_InvokeActionOnce()
    {
        var scope = new DisposeAction(Sut.Object);
        scope.Dispose();
        scope.Dispose();

        Sut.Verify(v => v(), Times.Once);
    }
}
