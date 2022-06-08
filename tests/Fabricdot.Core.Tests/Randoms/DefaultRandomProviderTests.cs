using System;
using Fabricdot.Core.Randoms;
using FluentAssertions;
using Xunit;

namespace Fabricdot.Core.Tests.Randoms;

public class DefaultRandomProviderTests
{
    protected DefaultRandomProvider RandomProvider { get; } = new();

    [Fact]
    public void Next_ReturnCorrectly()
    {
        var num1 = RandomProvider.Next();
        var num2 = RandomProvider.Next();

        num1.Should().NotBe(num2);
    }

    [InlineData(5, 1)]
    [Theory]
    public void Next_GivenInvalidInput_ThrowException(int min, int max)
    {
        FluentActions.Invoking(() => RandomProvider.Next(min, max))
                     .Should()
                     .Throw<ArgumentOutOfRangeException>();
    }
}