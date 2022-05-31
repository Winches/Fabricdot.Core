using System;
using Ardalis.GuardClauses;
using Fabricdot.Authorization;
using Fabricdot.Domain.Entities;
using Fabricdot.Domain.SharedKernel;

namespace Fabricdot.PermissionGranting.Domain;

public class GrantedPermission : AggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; private set; }

    public string GrantType { get; private set; }

    public string Subject { get; private set; }

    public string Object { get; private set; }

    internal GrantedPermission(
        Guid grantId,
        GrantSubject subject,
        string @object)
    {
        Id = Guard.Against.Default(grantId, nameof(grantId));
        GrantType = subject.Type;
        Subject = subject.Value;
        Object = Guard.Against.NullOrWhiteSpace(@object, nameof(@object));
    }

    internal GrantedPermission(
        Guid tenantId,
        Guid grantId,
        GrantSubject subject,
        string @object) : this(
            grantId,
            subject,
            @object)
    {
        TenantId = tenantId;
    }

    private GrantedPermission()
    {
    }
}