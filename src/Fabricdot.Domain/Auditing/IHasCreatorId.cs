namespace Fabricdot.Domain.Auditing;

public interface IHasCreatorId
{
    string? CreatorId { get; }
}