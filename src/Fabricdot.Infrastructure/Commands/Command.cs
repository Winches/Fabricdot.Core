using System.Text.Json.Serialization;
using MediatR;

namespace Fabricdot.Infrastructure.Commands;

public abstract class Command<TResult> : ICommand<TResult>, IRequest<TResult>
{
    [JsonIgnore]
    public Guid Id { get; }

    protected Command()
    {
        Id = Guid.NewGuid();
    }

    protected Command(Guid id)
    {
        Id = id;
    }
}

public abstract class Command : Command<object>
{
}
