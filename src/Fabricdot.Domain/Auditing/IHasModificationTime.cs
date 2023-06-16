namespace Fabricdot.Domain.Auditing;

public interface IHasModificationTime
{
    DateTime? LastModificationTime { get; }
}
