using Fabricdot.Infrastructure.Core.Tracing;
using Xunit;

namespace Fabricdot.Infrastructure.Core.Tests.Tracing
{
    public class CorrelationProviderTests
    {
        private readonly ICorrelationIdProvider _correlationIdProvider;
        public CorrelationProviderTests()
        {
            _correlationIdProvider = new DefaultCorrelationIdProvider();
        }

        [Fact]
        public void Get_ReturnCorrelationId()
        {
            var correlationId = _correlationIdProvider.Get();
            Assert.IsType<CorrelationId>(correlationId);
        }
    }
}