using Fabricdot.Infrastructure.Commands;

namespace Fabricdot.Infrastructure.Tests.Commands;

internal class AddValueCommand : Command<int>
{
    public int Left { get; }

    public int Right { get; }

    public AddValueCommand(
        int left,
        int right)
    {
        Left = left;
        Right = right;
    }
}

internal class AddValueCommandHandler : CommandHandler<AddValueCommand, int>
{
    public override Task<int> ExecuteAsync(
        AddValueCommand command,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(command.Left + command.Right);
    }
}