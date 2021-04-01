namespace Fabricdot.Infrastructure.Core.Tracing
{
    public interface ICorrelationIdProvider
    {
        CorrelationId Get();
    }
}
