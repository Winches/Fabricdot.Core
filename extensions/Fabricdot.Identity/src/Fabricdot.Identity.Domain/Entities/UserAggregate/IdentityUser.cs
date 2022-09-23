using Ardalis.GuardClauses;
using Fabricdot.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Fabricdot.Identity.Domain.Entities.UserAggregate;

public class IdentityUser : FullAuditAggregateRoot<Guid>
{
    private readonly List<IdentityUserClaim> _claims = new();
    private readonly List<IdentityUserRole> _roles = new();
    private readonly List<IdentityUserLogin> _logins = new();
    private readonly List<IdentityUserToken> _tokens = new();

    [ProtectedPersonalData]
    public virtual string UserName { get; protected internal set; } = null!;

    public virtual string NormalizedUserName { get; protected internal set; } = null!;

    [PersonalData]
    public virtual string? GivenName { get; set; }

    [PersonalData]
    public virtual string? Surname { get; set; }

    public virtual string? PasswordHash { get; protected internal set; }

    [ProtectedPersonalData]
    public virtual string? Email { get; protected internal set; }

    public virtual string? NormalizedEmail { get; protected internal set; }

    [PersonalData]
    public virtual bool EmailConfirmed { get; protected internal set; }

    [ProtectedPersonalData]
    public virtual string? PhoneNumber { get; protected set; }

    [PersonalData]
    public virtual bool PhoneNumberConfirmed { get; protected set; }

    [PersonalData]
    public virtual bool TwoFactorEnabled { get; protected internal set; }

    public bool IsActive { get; protected internal set; }

    public virtual bool LockoutEnabled { get; protected internal set; }

    public virtual DateTimeOffset? LockoutEnd { get; protected set; }

    public virtual int AccessFailedCount { get; protected set; }

    public virtual string SecurityStamp { get; protected internal set; } = null!;

    public virtual IReadOnlyCollection<IdentityUserClaim> Claims => _claims.AsReadOnly();

    public virtual IReadOnlyCollection<IdentityUserRole> Roles => _roles.AsReadOnly();

    public virtual IReadOnlyCollection<IdentityUserLogin> Logins => _logins.AsReadOnly();

    public virtual IReadOnlyCollection<IdentityUserToken> Tokens => _tokens.AsReadOnly();

    public virtual bool HasPassword => PasswordHash != null;

    public virtual bool IsLockedOut => LockoutEnabled && LockoutEnd.HasValue && LockoutEnd.Value >= DateTimeOffset.UtcNow;

    public IdentityUser(
        Guid userId,
        string userName)
    {
        Id = userId;
        UserName = Guard.Against.NullOrEmpty(userName?.Trim(), nameof(userName));
        NormalizedUserName = userName.Normalize().ToUpperInvariant();
        SecurityStamp = Guid.NewGuid().ToString("N");
        Enable();
    }

    public IdentityUser(
        Guid userId,
        string userName,
        string email) : this(userId, userName)
    {
        Email = Guard.Against.NullOrEmpty(email?.Trim(), nameof(email));
        NormalizedEmail = Email.Normalize().ToUpperInvariant();
    }

    protected IdentityUser()
    {
    }

    public virtual void ChangePhoneNumber(
        string? phoneNumber,
        bool isConfirmed)
    {
        ChangePhoneNumber(phoneNumber);
        PhoneNumberConfirmed = isConfirmed && !string.IsNullOrEmpty(phoneNumber);
    }

    public virtual void ChangePhoneNumber(string? phoneNumber) => PhoneNumber = phoneNumber?.Trim();

    public virtual int AccessFailed() => ++AccessFailedCount;

    public virtual void ResetAccessFailedCount() => AccessFailedCount = 0;

    public virtual void Lockout(DateTimeOffset lockoutEnd)
    {
        if (!LockoutEnabled)
            return;

        LockoutEnd = lockoutEnd;
    }

    public virtual void Unlock() => LockoutEnd = null;

    public virtual void AddRole(Guid roleId)
    {
        if (!IsInRole(roleId))
            _roles.Add(new IdentityUserRole(roleId));
    }

    public virtual void RemoveRole(Guid roleId)
    {
        _roles.RemoveAll(v => v.RoleId == roleId);
    }

    public virtual void ClearRoles() => _roles.Clear();

    public virtual bool IsInRole(Guid roleId)
    {
        return _roles.Any(v => v.RoleId == roleId);
    }

    public virtual void AddClaim(
        Guid claimId,
        string claimType,
        string? claimValue)
    {
        var claim = new IdentityUserClaim(claimId, claimType, claimValue);
        _claims.Add(claim);
    }

    public virtual void ReplaceClaim(
        string claimType,
        string? claimValue,
        string newClaimType,
        string? newClaimValue)
    {
        _claims.Where(v => v.ClaimType == claimType && v.ClaimValue == claimValue)
               .ForEach(v => v.SetClaim(newClaimType, newClaimValue));
    }

    public virtual void RemoveClaim(
        string claimType,
        string? claimValue)
    {
        _claims.RemoveAll(v => v.ClaimType == claimType && v.ClaimValue == claimValue);
    }

    public virtual void AddLogin(
        string loginProvider,
        string providerKey,
        string providerDisplayName)
    {
        var login = new IdentityUserLogin(
            loginProvider,
            providerKey,
            providerDisplayName);
        _logins.Add(login);
    }

    public virtual void RemoveLogin(
        string loginProvider,
        string providerKey)
    {
        _logins.RemoveAll(v => v.LoginProvider == loginProvider && v.ProviderKey == providerKey);
    }

    public virtual IdentityUserToken? FindToken(
        string loginProvider,
        string name)
    {
        return _tokens.SingleOrDefault(v => v.LoginProvider == loginProvider && v.Name == name);
    }

    public virtual void AddOrUpdateToken(
        string loginProvider,
        string name,
        string? value)
    {
        var token = FindToken(loginProvider, name);
        if (token is null)
        {
            _tokens.Add(new IdentityUserToken(loginProvider, name, value));
        }
        else
        {
            var newToken = token.ChangeValue(value);
            _tokens.Remove(token);
            _tokens.Add(newToken);
        }
    }

    public virtual void RemoveToken(
        string loginProvider,
        string name)
    {
        _tokens.RemoveAll(v => v.LoginProvider == loginProvider && v.Name == name);
    }

    public virtual void Enable() => IsActive = true;

    public virtual void Disable() => IsActive = false;

    public override string ToString() => UserName;
}