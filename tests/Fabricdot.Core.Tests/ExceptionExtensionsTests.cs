using System.Diagnostics.CodeAnalysis;
using Fabricdot.Core.Logging;
using Microsoft.Extensions.Logging;

namespace Fabricdot.Core.Tests;

public class ExceptionExtensionsTests : TestBase
{
    [SuppressMessage("Roslynator", "RCS1194:Implement exception constructors.", Justification = "<Pending>")]
    internal class ExceptionWithLogLevel : Exception, IHasLogLevel
    {
        /// <inheritdoc />
        public LogLevel LogLevel { get; set; }

        public ExceptionWithLogLevel(string message) : base(message)
        {
        }
    }

    [AutoData]
    [Theory]
    public void TryGetLogLevel_ExceptionWithoutLogLevel_ReturnGivenLogLevel(
        Exception exception,
        LogLevel expected)
    {
        exception.TryGetLogLevel(expected)
                 .Should()
                 .Be(expected);
    }

    [AutoData]
    [Theory]
    internal void TryGetLogLevel_ExceptionWithLogLevel_ReturnExceptionLogLevel(ExceptionWithLogLevel exception)
    {
        var expected = exception.LogLevel;

        exception.TryGetLogLevel()
                 .Should()
                 .Be(expected);
    }
}