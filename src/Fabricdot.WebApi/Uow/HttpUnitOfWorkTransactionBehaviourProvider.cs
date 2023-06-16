using Fabricdot.Core.DependencyInjection;
using Fabricdot.Infrastructure.Uow;
using Fabricdot.Infrastructure.Uow.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.WebApi.Uow;

[ServiceContract(typeof(IUnitOfWorkTransactionBehaviourProvider))]
[Dependency(ServiceLifetime.Singleton, RegisterBehavior = RegistrationBehavior.Replace)]
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
