using Telegram.Bot.Types;

namespace NattiDigestBot.Services;

public interface ICommandDispatcher
{
    Task HandleNormalMode(Message message, CancellationToken cancellationToken);
    Task HandleDigestMode(Message message, CancellationToken cancellationToken);
    Task HandleEditMode(Message message, CancellationToken cancellationToken);
    Task HandleWaitingForConfirmationMode(Message message, CancellationToken cancellationToken);
    Task HandleConfirmationFromGroup(Message message, CancellationToken cancellationToken);
    Task HandleMainMenuCallbackQuery(CallbackQuery query, CancellationToken cancellationToken);
    Task HandleHtmlReferenceMenuCallbackQuery(CallbackQuery query, CancellationToken cancellationToken);
}