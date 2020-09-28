namespace Fabricdot.Domain.Core.Auditing
{
    public interface IAuditEntity : IHasModificationTime
    {
        string LastModifierId { get; }
    }
}