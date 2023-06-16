using Fabricdot.Core.DependencyInjection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.Commands;

[Dependency(ServiceLifetime.Singleton)]
public class CommandBus : ICommandBus
{
    private readonly ISender _sender;

    public CommandBus(ISender sender)
    {
        _sender = sender;
    }

    public async Task<TResult> PublishAsync<TResult>(
        ICommand<TResult> command,
        CancellationToken cancellationToken = default)
    {
        var ret = await _sender.Send(command, cancellationToken);
        return (TResult)ret!;
    }
}
