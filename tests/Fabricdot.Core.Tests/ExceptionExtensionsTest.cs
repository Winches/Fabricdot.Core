using System;
using Fabricdot.Core.Logging;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Fabricdot.Core.Tests;

public class ExceptionExtensionsTest
{
    internal class ExceptionWithLogLevel : Exception, IHasLogLevel
    {
        /// <inheritdoc />
        public LogLevel LogLevel { get; set; }
    }

    [Fact]
    public void TryGetLogLevel_ExceptionWithoutLogLevel_ReturnGivenLogLevel()
    {
        const LogLevel expected = LogLevel.Trace;
        var exception = new Exception();
        var actual = exception.TryGetLogLevel(expected);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TryGetLogLevel_ExceptionWithLogLevel_ReturnExceptionLogLevel()
    {
        const LogLevel expected = LogLevel.Trace;
        var exception = new ExceptionWithLogLevel { LogLevel = expected };
        var actual = exception.TryGetLogLevel(expected);
        Assert.Equal(expected, actual);
    }
}