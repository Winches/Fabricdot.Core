namespace Fabricdot.Domain.Auditing;

public interface IAuditEntity : ICreationAuditEntity, IHasModificationTime, IHasModifierId
{
}
