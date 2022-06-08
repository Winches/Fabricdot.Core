using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;
using Fabricdot.Domain.Entities;

namespace Fabricdot.Identity.Domain.Entities.RoleAggregate;

public class IdentityRole : AggregateRoot<Guid>
{
    private readonly List<IdentityRoleClaim> _claims = new();
    public virtual string Name { get; protected internal set; } = null!;

    public virtual string NormalizedName { get; protected internal set; } = null!;

    public virtual string? Description { get; set; }

    public virtual IReadOnlyCollection<IdentityRoleClaim> Claims => _claims.AsReadOnly();

    public IdentityRole(
        Guid roleId,
        string roleName)
    {
        Id = roleId;
        Name = Guard.Against.NullOrEmpty(roleName?.Trim(), nameof(roleName));
        NormalizedName = Name.Normalize().ToUpperInvariant();
    }

    protected IdentityRole()
    {
    }

    public virtual void AddClaim(
        Guid claimId,
        string claimType,
        string claimValue)
    {
        var claim = new IdentityRoleClaim(claimId, claimType, claimValue);
        _claims.Add(claim);
    }

    public virtual void RemoveClaim(
        string claimType,
        string claimValue)
    {
        _claims.RemoveAll(v => v.ClaimType == claimType && v.ClaimValue == claimValue);
    }

    public override string ToString() => Name;
}