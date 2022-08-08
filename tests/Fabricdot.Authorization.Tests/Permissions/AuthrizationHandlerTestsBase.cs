using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Authorization.Tests.Permissions;

public class AuthrizationHandlerTestsBase<THandler> : AuthorizationTestBase where THandler : class, IAuthorizationHandler
{
    public IAuthorizationHandler AuthorizationHandler { get; }

    public AuthrizationHandlerTestsBase()
    {
        AuthorizationHandler = ServiceProvider.GetRequiredService<IEnumerable<IAuthorizationHandler>>()
                                              .OfType<THandler>()
                                              .Single();
    }
}