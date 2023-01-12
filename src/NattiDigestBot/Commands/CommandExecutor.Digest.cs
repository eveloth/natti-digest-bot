using NattiDigestBot.Commands.Interfaces;
using Telegram.Bot.Types;

namespace NattiDigestBot.Commands;

public partial class CommandExecutor : ICommandExecutor
{
    public Task AddEntry(Message message, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task RemoveEntry(Message message, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task RawPreview(Message message, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Make(Message message, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}