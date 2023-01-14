using Telegram.Bot.Types;

namespace NattiDigestBot.Commands.Interfaces;

public interface ICallbackQueryProcessor
{
    Task ShowGroupIdInfo(CallbackQuery query, CancellationToken cancellationToken);
    Task ShowGroupBindingInfo(CallbackQuery query, CancellationToken cancellationToken);
    Task ShowCategoriesInfo(CallbackQuery query, CancellationToken cancellationToken);
    Task ShowDigestInfo(CallbackQuery query, CancellationToken cancellationToken);
    Task ShowMyAccountId(CallbackQuery query, CancellationToken cancellationToken);
    Task ShowDeleteAccountPrompt(CallbackQuery query, CancellationToken cancellationToken);
    Task DeleteAccount(CallbackQuery query, CancellationToken cancellationToken);
    Task ShowPrivateGroupsInfo(CallbackQuery query, CancellationToken cancellationToken);
    Task BackToMain(CallbackQuery query, CancellationToken cancellationToken);
    Task ShowHtmlReference(CallbackQuery query, CancellationToken cancellationToken);
    Task BackToEdit(CallbackQuery query, CancellationToken cancellationToken);
}