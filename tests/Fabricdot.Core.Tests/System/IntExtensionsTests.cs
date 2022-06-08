using System;
using FluentAssertions;
using Xunit;

namespace Fabricdot.Core.Tests.System.Reflection;

public class IntExtensionsTests
{
    [Fact]
    public void Times_GivenNegativeNumber_ThrowException()
    {
        FluentActions.Invoking(() => (-1).Times(_ => { }))
                     .Should()
                     .Throw<ArgumentException>();
    }

    [Fact]
    public void Times_GivenInput_IteratingAction()
    {
        const int times = 3;
        var count = 0;
        times.Times(_ => count++);

        count.Should().Be(times);
    }
}