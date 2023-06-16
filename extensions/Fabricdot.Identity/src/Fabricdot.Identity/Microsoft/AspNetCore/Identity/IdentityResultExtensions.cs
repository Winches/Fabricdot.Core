using Ardalis.GuardClauses;

namespace Microsoft.AspNetCore.Identity;

public static class IdentityResultExtensions
{
    public static void EnsureSuccess(this IdentityResult result)
    {
        Guard.Against.Null(result, nameof(result));

        if (result.Succeeded)
            return;

        throw new InvalidOperationException(result.Errors.First().Description);
    }
}
