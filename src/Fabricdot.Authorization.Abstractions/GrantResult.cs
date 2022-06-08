using Ardalis.GuardClauses;

namespace Fabricdot.Authorization;

public class GrantResult
{
    public string Object { get; }

    public bool IsGranted { get; private set; }

    public GrantResult(
        string @object,
        bool isGranted = false)
    {
        Object = Guard.Against.NullOrWhiteSpace(@object.Trim(), nameof(@object));
        IsGranted = isGranted;
    }

    public static bool operator ==(GrantResult? left, GrantResult? right) => Equals(left, right);

    public static bool operator !=(GrantResult? left, GrantResult? right) => !Equals(left, right);

    public override bool Equals(object? obj) => obj is GrantResult result && Object == result.Object;

    public override int GetHashCode() => Object.GetHashCode();
}