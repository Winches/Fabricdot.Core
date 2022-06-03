namespace Fabricdot.Identity.Domain.SharedKernel
{
    public interface IIdentityClaim
    {
        string ClaimType { get; }

        string? ClaimValue { get; }
    }
}