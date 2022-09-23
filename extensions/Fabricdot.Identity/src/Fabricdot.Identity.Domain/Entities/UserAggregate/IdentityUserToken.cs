using Ardalis.GuardClauses;
using Fabricdot.Domain.ValueObjects;

namespace Fabricdot.Identity.Domain.Entities.UserAggregate;

public class IdentityUserToken : ValueObject
{
    public Guid UserId { get; private set; }

    public string LoginProvider { get; private set; } = null!;

    public string Name { get; private set; } = null!;

    public string? Value { get; private set; }

    public IdentityUserToken(
        string loginProvider,
        string tokeName,
        string? tokenValue)
    {
        LoginProvider = Guard.Against.NullOrEmpty(loginProvider, nameof(loginProvider));
        Name = Guard.Against.NullOrEmpty(tokeName, nameof(tokeName));
        Value = tokenValue;
    }

    private IdentityUserToken()
    {
    }

    public IdentityUserToken ChangeValue(string? tokenValue)
    {
        return new IdentityUserToken(LoginProvider, Name, tokenValue);
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return UserId;
        yield return LoginProvider;
        yield return Name;
        yield return Value;
    }
}