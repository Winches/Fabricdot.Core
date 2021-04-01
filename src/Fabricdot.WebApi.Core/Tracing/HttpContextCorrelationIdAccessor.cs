using Fabricdot.Infrastructure.Core.Tracing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Fabricdot.WebApi.Core.Tracing
{
    public class HttpContextCorrelationIdAccessor : ICorrelationIdAccessor
    {
        private readonly CorrelationIdOptions _options;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextCorrelationIdAccessor(
            IOptions<CorrelationIdOptions> options,
            IHttpContextAccessor httpContextAccessor)
        {
            _options = options.Value;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <inheritdoc />
        public CorrelationId? CorrelationId
        {
            get
            {
                var context = _httpContextAccessor.HttpContext;
                if (context == null)
                    return null;
                context.Request.Headers.TryGetValue(_options.HeaderKey, out var correlationId);
                return correlationId.ToString();
            }
        }
    }
}