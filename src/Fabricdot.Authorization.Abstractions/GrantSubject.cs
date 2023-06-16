using System.Security.Claims;
using Ardalis.GuardClauses;

namespace Fabricdot.Authorization;

public struct GrantSubject
{
    public string Type { get; }

    public string Value { get; }

    public GrantSubject(
        string type,
        string value)
    {
        Type = Guard.Against.NullOrWhiteSpace(type, nameof(type));
        Value = Guard.Against.NullOrWhiteSpace(value, nameof(value));
    }

    public static implicit operator GrantSubject(Claim claim)
    {
        Guard.Against.Null(claim, nameof(claim));

        return new(claim.Type, claim.Value);
    }

    public static GrantSubject User(string userId) => new(GrantTypes.User, userId);

    public static GrantSubject Role(string role) => new(GrantTypes.Role, role);
}
