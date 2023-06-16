namespace Fabricdot.Infrastructure.Commands;

[Obsolete("Use Command", true)]
public class CommandBase : ICommand
{
    public Guid Id { get; }

    public CommandBase()
    {
        Id = Guid.NewGuid();
    }

    protected CommandBase(Guid id)
    {
        Id = id;
    }
}

[Obsolete("Use Command", true)]
public abstract class CommandBase<TResult> : ICommand<TResult>
{
    public Guid Id { get; }

    protected CommandBase()
    {
        Id = Guid.NewGuid();
    }

    protected CommandBase(Guid id)
    {
        Id = id;
    }
}
