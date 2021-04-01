using Fabricdot.Infrastructure.Core.Tracing;

namespace WorkerService.WebApi.Services.Tracing
{
    public class DefaultCorrelationIdProvider : ICorrelationIdProvider
    {
        public CorrelationId Get() => CorrelationId.New();
    }
}