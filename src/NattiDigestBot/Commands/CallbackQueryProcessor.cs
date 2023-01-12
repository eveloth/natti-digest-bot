using NattiDigestBot.Commands.Interfaces;
using NattiDigestBot.Extensions;
using NattiDigestBot.Replies;
using NattiDigestBot.Replies.Menus;
using NattiDigestBot.Services.DbServices;
using NattiDigestBot.State;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace NattiDigestBot.Commands;

public class CallbackQueryProcessor : ICallbackQueryProcessor
{
    private readonly ILogger<CallbackQueryProcessor> _logger;
    private readonly ITelegramBotClient _botClient;
    private readonly IAccountService _accountService;

    public CallbackQueryProcessor(
        ILogger<CallbackQueryProcessor> logger,
        ITelegramBotClient botClient,
        IAccountService accountService
    )
    {
        _logger = logger;
        _botClient = botClient;
        _accountService = accountService;
    }

    public async Task ShowGroupIdInfo(CallbackQuery query, CancellationToken cancellationToken)
    {
        var userId = query.From.Id;
        var messageId = query.Message!.MessageId;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(ShowGroupIdInfo),
            userId
        );

        await _botClient.EditReply(userId, messageId, HelpMenu.GroupIdSection, cancellationToken);
    }

    public async Task ShowGroupBindingInfo(CallbackQuery query, CancellationToken cancellationToken)
    {
        var userId = query.From.Id;
        var messageId = query.Message!.MessageId;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(ShowGroupBindingInfo),
            userId
        );

        await _botClient.EditReply(userId, messageId, HelpMenu.BindSection, cancellationToken);
    }

    public async Task ShowCategoriesInfo(CallbackQuery query, CancellationToken cancellationToken)
    {
        var userId = query.From.Id;
        var messageId = query.Message!.MessageId;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(ShowCategoriesInfo),
            userId
        );

        await _botClient.EditReply(userId, messageId, HelpMenu.CategorySection, cancellationToken);
    }

    public async Task ShowDigestInfo(CallbackQuery query, CancellationToken cancellationToken)
    {
        var userId = query.From.Id;
        var messageId = query.Message!.MessageId;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(ShowDigestInfo),
            userId
        );

        await _botClient.EditReply(userId, messageId, HelpMenu.DigestSection, cancellationToken);
    }

    public async Task ShowMyAccountId(CallbackQuery query, CancellationToken cancellationToken)
    {
        var userId = query.From.Id;
        var messageId = query.Message!.MessageId;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(ShowMyAccountId),
            userId
        );

        var reply = HelpMenu.AccountIdSection with
        {
            ReplyText = $"ID твоего аккаунта: <b>{userId}</b>"
        };

        await _botClient.EditReply(userId, messageId, reply, cancellationToken);
    }

    public async Task ShowDeleteAccountPrompt(
        CallbackQuery query,
        CancellationToken cancellationToken
    )
    {
        var userId = query.From.Id;
        var messageId = query.Message!.MessageId;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(ShowDeleteAccountPrompt),
            userId
        );

        await _botClient.EditReply(
            userId,
            messageId,
            HelpMenu.DeleteAccountSection,
            cancellationToken
        );
    }

    public async Task DeleteAccount(CallbackQuery query, CancellationToken cancellationToken)
    {
        var userId = query.From.Id;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(DeleteAccount),
            userId
        );

        var account = await _accountService.Get(userId, cancellationToken);

        if (account is null)
        {
            return;
        }

        StateStorage.DropState(userId, account.GroupId);
        await _accountService.Delete(account.AccountId, cancellationToken);

        await _botClient.SendReply(userId, GeneralReplies.AccountDeletedReply, cancellationToken);
    }

    public async Task BackToMain(CallbackQuery query, CancellationToken cancellationToken)
    {
        var userId = query.From.Id;
        var messageId = query.Message!.MessageId;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(BackToMain),
            userId
        );

        await _botClient.EditReply(userId, messageId, HelpMenu.Entrypoint, cancellationToken);
    }
}