namespace System.Security.Claims;

public static class ClaimsIdentityExtenisons
{
    public static Claim GetOrAdd(
        this ClaimsIdentity claimsIdentity,
        string claimType,
        string claimValue)
    {
        if (claimsIdentity == null)
            throw new ArgumentNullException(nameof(claimsIdentity));

        var claim = claimsIdentity.FindFirst(claimType);
        if (claim == null)
        {
            claim = new Claim(claimType, claimValue);
            claimsIdentity.AddClaim(claim);
        }
        return claim;
    }

    public static Claim GetOrAdd(
        this ClaimsIdentity claimsIdentity,
        Claim claim)
    {
        if (claimsIdentity == null)
            throw new ArgumentNullException(nameof(claimsIdentity));

        return claimsIdentity.GetOrAdd(claim.Type, claim.Value);
    }
}
