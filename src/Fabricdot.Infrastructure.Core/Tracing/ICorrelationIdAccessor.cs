namespace Fabricdot.Infrastructure.Core.Tracing
{
    public interface ICorrelationIdAccessor
    {
        CorrelationId? CorrelationId { get; }
    }
}