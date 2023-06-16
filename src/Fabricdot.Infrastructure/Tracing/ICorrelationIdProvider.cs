namespace Fabricdot.Infrastructure.Tracing;

public interface ICorrelationIdProvider
{
    CorrelationId Get();
}
