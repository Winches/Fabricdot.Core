namespace Fabricdot.Domain.Core.Auditing
{
    public interface ICreationAuditEntity : IHasCreationTime
    {
        string CreationId { get; }
    }
}