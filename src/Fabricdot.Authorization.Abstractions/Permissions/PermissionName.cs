using System;
using Ardalis.GuardClauses;

namespace Fabricdot.Authorization.Permissions;

public struct PermissionName
{
    public string Value { get; }

    public PermissionName(string value)
    {
        Value = Guard.Against.NullOrWhiteSpace(value, nameof(value));
    }

    public static bool operator ==(PermissionName left, PermissionName right) => left.Equals(right);

    public static bool operator !=(PermissionName left, PermissionName right) => !(left == right);

    public static implicit operator PermissionName(string value) => new(value);

    public static implicit operator string(PermissionName value) => value.Value;

    public override bool Equals(object? obj) => obj is PermissionName name
                                               && Value.Equals(name.Value, StringComparison.InvariantCultureIgnoreCase);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value;
}