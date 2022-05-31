using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Fabricdot.Authorization;
using Fabricdot.Core.DependencyInjection;
using Fabricdot.Core.UniqueIdentifier;
using Fabricdot.PermissionGranting.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.PermissionGranting;

[Dependency(ServiceLifetime.Scoped)]
public class PermissionGrantingManager : IPermissionGrantingManager
{
    protected IGrantedPermissionRepository PermissionGrantigRepository { get; }
    protected IGuidGenerator GuidGenerator { get; }
    protected IGrantSubjectResolver GrantSubjectResolver { get; }

    public PermissionGrantingManager(
        IGrantedPermissionRepository permissionGrantigRepository,
        IGuidGenerator guidGenerator,
        IGrantSubjectResolver grantSubjectResolver)
    {
        PermissionGrantigRepository = permissionGrantigRepository;
        GuidGenerator = guidGenerator;
        GrantSubjectResolver = grantSubjectResolver;
    }

    public virtual async Task GrantAsync(
        GrantSubject subject,
        string @object,
        CancellationToken cancellationToken = default)
    {
        if (await PermissionGrantigRepository.AnyAsync(
            subject,
            @object,
            cancellationToken))
        {
            return;
        }

        var granting = new GrantedPermission(
            GuidGenerator.Create(),
            subject,
            @object);
        await PermissionGrantigRepository.AddAsync(granting, cancellationToken);
    }

    public virtual async Task RevokeAsync(
        GrantSubject subject,
        string @object,
        CancellationToken cancellationToken = default)
    {
        var granting = await PermissionGrantigRepository.GetAsync(
                        subject,
                        @object,
                        cancellationToken);
        if (granting == null)
            return;

        await PermissionGrantigRepository.DeleteAsync(granting, cancellationToken);
    }

    public async Task SetAsync(
        GrantSubject subject,
        IEnumerable<string> objects,
        CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(objects, nameof(objects));

        objects = objects.Distinct();
        var grantedPermissions = await PermissionGrantigRepository.ListAsync(subject, cancellationToken);
        var grantedObjects = grantedPermissions.Select(v => v.Object).ToList();

        foreach (var @object in objects.Except(grantedObjects))
        {
            var grantedPermission = new GrantedPermission(GuidGenerator.Create(), subject, @object);
            await PermissionGrantigRepository.AddAsync(grantedPermission, cancellationToken);
        }
        foreach (var @object in grantedObjects.Except(objects))
        {
            var grantedPermission = grantedPermissions.Single(v => v.Object == @object);
            await PermissionGrantigRepository.DeleteAsync(grantedPermission, cancellationToken);
        }
    }

    public virtual async Task<IReadOnlyCollection<GrantedPermission>> ListAsync(
        GrantSubject subject,
        CancellationToken cancellationToken = default)
    {
        return await PermissionGrantigRepository.ListAsync(
            subject,
            cancellationToken);
    }

    public virtual async Task<IReadOnlyCollection<GrantedPermission>> ListAsync(
        ICollection<GrantSubject> subjects,
        CancellationToken cancellationToken = default)
    {
        return await PermissionGrantigRepository.ListAsync(
            subjects,
            cancellationToken);
    }

    public virtual async Task<IReadOnlyCollection<GrantedPermission>> ListAsync(
        ClaimsPrincipal claimsPrincipal,
        CancellationToken cancellationToken = default)
    {
        var subjects = await GrantSubjectResolver.ResolveAsync(claimsPrincipal, cancellationToken);
        return await PermissionGrantigRepository.ListAsync(subjects, cancellationToken);
    }
}