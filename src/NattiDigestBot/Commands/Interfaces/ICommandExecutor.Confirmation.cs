using Telegram.Bot.Types;

namespace NattiDigestBot.Commands.Interfaces;

public partial interface ICommandExecutor
{
    Task ConfirmGroup(Message message, CancellationToken cancellationToken);
    Task HandleMessageInConfirmationMode(Message message, CancellationToken cancellationToken);
}