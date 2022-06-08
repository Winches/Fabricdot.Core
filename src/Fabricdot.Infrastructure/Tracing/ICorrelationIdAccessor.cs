namespace Fabricdot.Infrastructure.Tracing;

public interface ICorrelationIdAccessor
{
    CorrelationId? CorrelationId { get; }
}