namespace Fabricdot.Core.Tests.System.Reflection;

public class IntExtensionsTests : TestBase
{
    [Fact]
    public void Times_GivenNegativeNumber_ThrowException()
    {
        Invoking(() => (-1).Times(_ => { }))
                     .Should()
                     .Throw<ArgumentException>();
    }

    [Fact]
    public void Times_GivenInput_IteratingAction()
    {
        var times = Create<int>();
        var actionMock = Mock<Action<int>>();
        times.Times(actionMock.Object);

        actionMock.Verify(v => v(It.IsAny<int>()), Times.Exactly(times));
    }
}
