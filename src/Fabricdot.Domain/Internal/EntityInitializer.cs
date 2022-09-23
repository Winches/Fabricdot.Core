namespace Fabricdot.Domain.Internal;

public class EntityInitializer
{
    public static readonly EntityInitializer Instance = new();

    public HashSet<IEntityInitializer> Initializers { get; } = new HashSet<IEntityInitializer>();

    public void Initialize(object entity)
    {
        Initializers.ForEach(v => v.Initialize(entity));
    }

    public void Add<T>() where T : IEntityInitializer, new()
    {
        Initializers.Add(new T());
    }
}