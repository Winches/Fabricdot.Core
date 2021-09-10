using System;
using System.Net.Http;
using Fabricdot.Infrastructure.Core.Uow;
using Microsoft.AspNetCore.Http;

namespace Fabricdot.WebApi.Core.Uow
{
    public class HttpUnitOfWorkTransactionBehaviourProvider : DefaultUnitOfWorkTransactionBehaviourProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpUnitOfWorkTransactionBehaviourProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <inheritdoc />
        public override bool GetBehaviour(string actionName)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var requestUrl = httpContext?.Request.Path.Value;
            if (httpContext != null && !string.IsNullOrEmpty(requestUrl))
            {
                var method = httpContext.Request.Method;
                return !string.Equals(method, HttpMethod.Get.Method, StringComparison.OrdinalIgnoreCase);
            }

            return base.GetBehaviour(actionName);
        }
    }
}