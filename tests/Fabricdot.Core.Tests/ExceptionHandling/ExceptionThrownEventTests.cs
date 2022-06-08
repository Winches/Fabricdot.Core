using System;
using System.Diagnostics.CodeAnalysis;
using Fabricdot.Core.ExceptionHandling;
using Fabricdot.Core.Logging;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Fabricdot.Core.Tests.ExceptionHandling;

public class ExceptionThrownEventTests
{
    [SuppressMessage("Roslynator", "RCS1194:Implement exception constructors.", Justification = "<Pending>")]
    private class FakeExceptionWithLogLevel : Exception, IHasLogLevel
    {
        public LogLevel LogLevel { get; }

        public FakeExceptionWithLogLevel(LogLevel logLevel)
        {
            LogLevel = logLevel;
        }
    }

    [Fact]
    public void Constructor_GivenNullException_ThrowException()
    {
        FluentActions.Invoking(() => new ExceptionThrownEvent(null, null))
                     .Should()
                     .Throw<ArgumentNullException>();
    }

    [Fact]
    public void Constructor_GivenInput_SetVale()
    {
        var exception = new Exception();
        var logLevel = LogLevel.Error;
        var exceptionThrownEvent = new ExceptionThrownEvent(exception, logLevel);

        exceptionThrownEvent.Exception.Should().Be(exception);
        exceptionThrownEvent.LogLevel.Should().Be(logLevel);
    }

    [Fact]
    public void Constructor_GivenNullLogLevel_TrySetLogLevel()
    {
        var expected = LogLevel.Error;
        var exceptionThrownEvent = new ExceptionThrownEvent(new FakeExceptionWithLogLevel(expected), null);

        exceptionThrownEvent.LogLevel.Should().Be(expected);
    }
}