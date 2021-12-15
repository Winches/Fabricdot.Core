namespace Fabricdot.Infrastructure.Tracing
{
    public class DefaultCorrelationIdProvider : ICorrelationIdProvider
    {
        public CorrelationId Get() => CorrelationId.New();
    }
}