using System.Collections.Immutable;
using System.Linq.Expressions;
using Ardalis.GuardClauses;
using Fabricdot.Authorization;
using Fabricdot.Infrastructure.EntityFrameworkCore;
using Fabricdot.PermissionGranting.Domain;
using Microsoft.EntityFrameworkCore;

namespace Fabricdot.PermissionGranting.Infrastructure.Data;

public class GrantedPermissionRepository<TDbContext> : EfRepository<TDbContext, GrantedPermission, Guid>, IGrantedPermissionRepository
    where TDbContext : DbContext, IPermissionGrantingDbContext
{
    public GrantedPermissionRepository(IDbContextProvider<TDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<bool> AnyAsync(
        GrantSubject subject,
        string @object,
        CancellationToken cancellationToken = default)
    {
        Guard.Against.NullOrEmpty(@object, nameof(@object));

        var query = await GetQueryableAsync(cancellationToken);
        var count = await query.CountAsync(
            v => v.GrantType == subject.Type && v.Subject == subject.Value && v.Object == @object,
            cancellationToken: cancellationToken);
        return count > 0;
    }

    public virtual async Task<GrantedPermission?> GetAsync(
        GrantSubject subject,
        string @object,
        CancellationToken cancellationToken = default)
    {
        Guard.Against.NullOrEmpty(@object, nameof(@object));

        var query = await GetQueryableAsync(cancellationToken: cancellationToken);
        return await query.SingleOrDefaultAsync(
            v => v.GrantType == subject.Type && v.Subject == subject.Value && v.Object == @object,
            cancellationToken: cancellationToken);
    }

    public virtual async Task<IReadOnlyCollection<GrantedPermission>> ListAsync(
        GrantSubject subject,
        CancellationToken cancellationToken = default)
    {
        var query = await GetQueryableAsync(cancellationToken: cancellationToken);
        var list = await query.Where(v => v.GrantType == subject.Type && v.Subject == subject.Value)
                              .ToListAsync(cancellationToken);
        return list.AsReadOnly();
    }

    public virtual async Task<IReadOnlyCollection<GrantedPermission>> ListAsync(
        GrantSubject subject,
        IEnumerable<string> objects,
        CancellationToken cancellationToken = default)
    {
        Guard.Against.NullOrEmpty(objects, nameof(objects));

        var query = await GetQueryableAsync(cancellationToken: cancellationToken);
        var list = await query.Where(v => v.GrantType == subject.Type && v.Subject == subject.Value && objects.Contains(v.Object))
                              .ToListAsync(cancellationToken);
        return list.AsReadOnly();
    }

    public virtual async Task<IReadOnlyCollection<GrantedPermission>> ListAsync(
        IEnumerable<GrantSubject> subjects,
        CancellationToken cancellationToken = default)
    {
        Guard.Against.NullOrEmpty(subjects, nameof(subjects));

        var clauses = subjects.Select(v => (Expression<Func<GrantedPermission, bool>>)((o) => o.GrantType == v.Type && o.Subject == v.Value))
                              .ToArray();
        var predicate = clauses.ComposeOr();

        var query = await GetQueryableAsync(cancellationToken: cancellationToken);

        return await query.Where(predicate).ToListAsync(cancellationToken);
    }
}
