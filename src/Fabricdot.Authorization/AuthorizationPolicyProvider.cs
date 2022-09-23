using Fabricdot.Core.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fabricdot.Authorization;

[Dependency(ServiceLifetime.Singleton)]
[ServiceContract(typeof(IAuthorizationPolicyProvider))]
public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
    private readonly AuthorizationOptions _options;
    private readonly ILogger<AuthorizationPolicyProvider> _logger;

    public AuthorizationPolicyProvider(
        IOptions<AuthorizationOptions> options,
        ILogger<AuthorizationPolicyProvider> logger)
        : base(options)
    {
        _options = options.Value;
        _logger = logger;
    }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var policy = await base.GetPolicyAsync(policyName);
        if (policy != null)
        {
            _logger.LogDebug($"Find authorization policy:{policyName}");
            return policy;
        }

        policy = new AuthorizationPolicyBuilder().RequirePermission(policyName)
                                                 .Build();

        //cache policy
        _options.AddPolicy(policyName, policy);
        _logger.LogDebug($"Build authorization policy:{policyName}");

        return policy;
    }
}