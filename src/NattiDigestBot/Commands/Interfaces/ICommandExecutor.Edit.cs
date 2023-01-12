using Telegram.Bot.Types;

namespace NattiDigestBot.Commands.Interfaces;

public partial interface ICommandExecutor
{
    Task UpdateDigestText(Message message, CancellationToken cancellationToken);
}