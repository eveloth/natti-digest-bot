using Telegram.Bot.Types;

namespace NattiDigestBot.Commands.Interfaces;

public partial interface ICommandExecutor
{
    Task SendUsage(Message message, CancellationToken cancellationToken);
    Task SendHelp(Message message, CancellationToken cancellationToken);
    Task ExitAndSetNormalMode(Message message, CancellationToken cancellationToken);
    Task UnsupportedCommand(Message message, CancellationToken cancellationToken);
}