namespace Fabricdot.Infrastructure.Data.Filters;

public class DataFilterOptions
{
    public Dictionary<Type, DataFilterState> DefaultStates { get; } = new Dictionary<Type, DataFilterState>();
}
