using System.Diagnostics.CodeAnalysis;
using Fabricdot.Core.ExceptionHandling;
using Fabricdot.Core.Logging;
using Microsoft.Extensions.Logging;

namespace Fabricdot.Core.Tests.ExceptionHandling;

public class ExceptionThrownEventTests : TestBase
{
    [SuppressMessage("Roslynator", "RCS1194:Implement exception constructors.", Justification = "<Pending>")]
    public class FakeExceptionWithLogLevel : Exception, IHasLogLevel
    {
        public LogLevel LogLevel { get; set; }
    }

    [Fact]
    public void Constructor_GivenNullException_ThrowException()
    {
        var sut = typeof(ExceptionThrownEvent).GetConstructors();

        Create<GuardClauseAssertion>().Verify(sut);
    }

    [Fact]
    public void Constructor_GivenInput_SetVale()
    {
        var sut = typeof(ExceptionThrownEvent);

        Create<ConstructorInitializedMemberAssertion>().Verify(sut);
    }

    [AutoData]
    [Theory]
    public void Constructor_GivenNullLogLevel_TrySetLogLevel(FakeExceptionWithLogLevel exception)
    {
        var expected = exception.LogLevel;
        var exceptionThrownEvent = new ExceptionThrownEvent(exception, null);

        exceptionThrownEvent.LogLevel.Should().Be(expected);
    }
}