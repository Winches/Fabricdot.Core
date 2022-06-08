namespace Fabricdot.Domain.Auditing;

public interface ISoftDelete
{
    bool IsDeleted { get; }
}