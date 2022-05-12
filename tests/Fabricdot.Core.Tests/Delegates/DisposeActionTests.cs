using System;
using Fabricdot.Core.Delegates;
using FluentAssertions;
using Xunit;

namespace Fabricdot.Core.Tests.Delegates
{
    public class DisposeActionTests
    {
        [Fact]
        public void Constructor_GivenNull_TrowException()
        {
            FluentActions.Invoking(() => new DisposeAction(null))
                         .Should()
                         .Throw<ArgumentException>();
        }

        [Fact]
        public void Dispose_GivenAction_InvokeAction()
        {
            var isCalled = false;
            using (var scope = new DisposeAction(() => isCalled = true))
            {
                isCalled.Should().BeFalse();
            }
            isCalled.Should().BeTrue();
        }

        [Fact]
        public void Dispose_InvokeTwice_InvokeActionOnce()
        {
            var count = 0;
            var scope = new DisposeAction(() => count++);
            scope.Dispose();
            scope.Dispose();
            count.Should().Be(1);
        }
    }
}