using System;
using Fabricdot.Infrastructure.Tracing;
using Xunit;

namespace Fabricdot.Infrastructure.Tests.Tracing;

public class CorrelationIdTests
{
    [Theory]
    [InlineData(" ")]
    [InlineData("")]
    [InlineData(null)]
    public void ImplicitOperator_GivenNullOrEmptyValue_ThrowException(string value)
    {
        void Action()
        {
            var _ = (CorrelationId)value;
        }

        Assert.Throws<ArgumentException>(Action);
    }

    [Fact]
    public void Equals_GivenSameValue_ReturnTrue()
    {
        var correlationId1 = CorrelationId.New();
        var correlationId2 = (CorrelationId)correlationId1.Value;
        Assert.Equal(correlationId1, correlationId2);
    }

    [Fact]
    public void Equals_GivenDifferentValue_ReturnFalse()
    {
        var correlationId1 = CorrelationId.New();
        var correlationId2 = CorrelationId.New();
        Assert.NotEqual(correlationId1, correlationId2);
    }

    [Fact]
    public void ToString_ReturnValue()
    {
        var correlationId = CorrelationId.New();
        var expected = correlationId.Value;
        var actual = correlationId.ToString();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void New_ReturnNewInstance()
    {
        var correlationId = CorrelationId.New();
        Assert.NotEmpty(correlationId.Value);
    }
}