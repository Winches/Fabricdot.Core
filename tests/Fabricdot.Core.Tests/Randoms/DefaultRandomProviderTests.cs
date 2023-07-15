using Fabricdot.Core.Randoms;

namespace Fabricdot.Core.Tests.Randoms;

public class DefaultRandomProviderTests : TestFor<DefaultRandomProvider>
{
    [Fact]
    public void Next_ReturnCorrectly()
    {
        Sut.Next().Should().BeInRange(0, int.MaxValue);
    }

    [InlineData(5, 1)]
    [Theory]
    public void Next_GivenInvalidInput_ThrowException(int min, int max)
    {
        Invoking(() => Sut.Next(min, max))
                     .Should()
                     .Throw<ArgumentOutOfRangeException>();
    }
}
