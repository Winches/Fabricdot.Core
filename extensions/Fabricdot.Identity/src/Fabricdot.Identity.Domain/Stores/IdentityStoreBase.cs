using Fabricdot.Core.UniqueIdentifier;

namespace Fabricdot.Identity.Domain.Stores;

public abstract class IdentityStoreBase : IDisposable
{
    public bool AutoSaveChanges { get; set; }
    protected IGuidGenerator GuidGenerator { get; }

    protected IdentityStoreBase(IGuidGenerator guidGenerator)
    {
        GuidGenerator = guidGenerator;
    }

    public virtual void Dispose()
    {
    }

    public virtual Guid ConvertIdFromString(string id) => Guid.Parse(id);

    public virtual string? ConvertIdToString(Guid id) => (id == default) ? null : id.ToString();

    protected abstract Task SaveChangesAsync(CancellationToken cancellationToken);
}