namespace Fabricdot.Domain.Core.Auditing
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; }
    }
}