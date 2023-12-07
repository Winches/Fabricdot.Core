using Fabricdot.Infrastructure.Tracing;

namespace Fabricdot.Infrastructure.Tests.Tracing;

public class CorrelationIdTests : TestFor<CorrelationId>
{
    [Theory]
    [InlineData(" ")]
    [InlineData("")]
    [InlineData(null)]
    public void ImplicitOperator_GivenNullOrEmptyValue_ThrowException(string? value)
    {
        Invoking(() => (CorrelationId)value!)
                     .Should()
                     .Throw<ArgumentException>();
    }

    [Fact]
    public void Equals_GivenSameValue_ReturnTrue()
    {
        var correlationId = (CorrelationId)Sut.Value;

        Sut.Should().Be(correlationId);
    }

    [Fact]
    public void Equals_GivenDifferentValue_ReturnFalse()
    {
        var correlationId = CorrelationId.New();

        Sut.Should().NotBe(correlationId);
    }

    [Fact]
    public void ToString_ReturnValue()
    {
        var expected = Sut.Value;

        Sut.ToString().Should().Be(expected);
    }

    [Fact]
    public void New_ReturnNewInstance()
    {
        Sut.Value.Should().NotBeEmpty();
    }

    protected override CorrelationId CreateSut() => CorrelationId.New();
}
