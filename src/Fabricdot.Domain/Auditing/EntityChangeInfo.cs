namespace Fabricdot.Domain.Auditing;

public class EntityChangeInfo
{
    public object Entity { get; }

    public EntityStatus Status { get; }

    public EntityChangeInfo(
        object entity,
        EntityStatus status)
    {
        Entity = entity;
        Status = status;
    }
}
