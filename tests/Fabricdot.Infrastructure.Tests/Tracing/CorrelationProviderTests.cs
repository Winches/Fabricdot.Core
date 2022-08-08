using Fabricdot.Infrastructure.Tracing;

namespace Fabricdot.Infrastructure.Tests.Tracing;

public class CorrelationProviderTests : TestFor<DefaultCorrelationIdProvider>
{
    [Fact]
    public void Get_ReturnCorrelationId()
    {
        var correlationId = Sut.Get();

        Sut.Get().Should().NotBe(correlationId);
    }
}