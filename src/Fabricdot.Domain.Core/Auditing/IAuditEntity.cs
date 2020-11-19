namespace Fabricdot.Domain.Core.Auditing
{
    public interface IAuditEntity : ICreationAuditEntity, IHasModificationTime, IHasModifierId
    {
    }
}