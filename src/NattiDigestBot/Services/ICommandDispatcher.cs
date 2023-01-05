using Telegram.Bot.Types;

namespace NattiDigestBot.Services;

public interface ICommandDispatcher
{
    Task HandleNormalMode(Message message, CancellationToken cancellationToken);
    Task HandleDigestMode(Message message, CancellationToken cancellationToken);
    Task HandleEditMode(Message message, CancellationToken cancellationToken);
    Task HandleWaitingForConfirmationMode(long groupId, CancellationToken cancellationToken);
}