namespace Fabricdot.Infrastructure.Core.Tracing
{
    public class DefaultCorrelationIdProvider : ICorrelationIdProvider
    {
        public CorrelationId Get() => CorrelationId.New();
    }
}