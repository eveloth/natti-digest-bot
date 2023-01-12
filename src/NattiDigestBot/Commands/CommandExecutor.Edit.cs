using NattiDigestBot.Commands.Interfaces;
using Telegram.Bot.Types;

namespace NattiDigestBot.Commands;

public partial class CommandExecutor : ICommandExecutor
{
    public Task UpdateDigestText(Message message, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}