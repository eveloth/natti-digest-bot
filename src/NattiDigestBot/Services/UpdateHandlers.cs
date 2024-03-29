using NattiDigestBot.Domain;
using NattiDigestBot.Replies.Menus;
using NattiDigestBot.Services.DbServices;
using NattiDigestBot.State;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace NattiDigestBot.Services;

public class UpdateHandlers
{
    private readonly ILogger<UpdateHandlers> _logger;
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IAccountService _accountService;
    private readonly ITelegramBotClient _botClient;

    public UpdateHandlers(
        ILogger<UpdateHandlers> logger,
        ICommandDispatcher commandDispatcher,
        IAccountService accountService,
        ITelegramBotClient botClient
    )
    {
        _logger = logger;
        _commandDispatcher = commandDispatcher;
        _accountService = accountService;
        _botClient = botClient;
    }

    public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
    {
        var handler = update switch
        {
            { Message: { } message } => BotOnMessageReceived(message, cancellationToken),
            { CallbackQuery: { } callbackQuery }
                => BotOnCallbackQueryReceived(callbackQuery, cancellationToken),
            _ => UnknownUpdateHandlerAsync(update, cancellationToken)
        };

        await handler;
    }

    private async Task BotOnMessageReceived(Message message, CancellationToken cancellationToken)
    {
        var chatId = message.Chat.Id;
        _logger.LogInformation("Received message from chat ID {ChatId}", chatId);

        if (!MessageIsText(message))
        {
            return;
        }

        if (MessageIsForwarded(message))
        {
            return;
        }

        if (!MessageIsFromPrivateChat(message))
        {
            if (!StateStorage.IsWaitingForConfirmation(chatId))
            {
                return;
            }

            await _commandDispatcher.HandleConfirmationFromGroup(message, cancellationToken);
            return;
        }

        var userId = chatId;
        var account = await _accountService.Get(userId, cancellationToken);

        if (account is null)
        {
            var newAccount = new Account { AccountId = userId };

            await _accountService.Create(newAccount, cancellationToken);
            StateStorage.SetChatModeFor(userId, ChatMode.Normal);
        }

        var currentMode = StateStorage.GetChatMode(userId);

        var action = currentMode switch
        {
            ChatMode.Normal => _commandDispatcher.HandleNormalMode(message, cancellationToken),
            ChatMode.Digest => _commandDispatcher.HandleDigestMode(message, cancellationToken),
            ChatMode.Edit => _commandDispatcher.HandleEditMode(message, cancellationToken),
            ChatMode.WaitingForConfirmation
                => _commandDispatcher.HandleWaitingForConfirmationMode(message, cancellationToken),
            _ => Task.CompletedTask
        };

        await action;
    }

    private async Task BotOnCallbackQueryReceived(
        CallbackQuery callbackQuery,
        CancellationToken cancellationToken
    )
    {
        if (callbackQuery.From.IsBot)
            return;

        var callbackHeader = callbackQuery.Data!.Split(':')[0];

        var action = callbackHeader switch
        {
            _ when callbackHeader.Equals(CallbackData.Main)
                => _commandDispatcher.HandleMainMenuCallbackQuery(callbackQuery, cancellationToken),
            _ when callbackHeader.Equals(CallbackData.Html)
                => _commandDispatcher.HandleHtmlReferenceMenuCallbackQuery(
                    callbackQuery,
                    cancellationToken
                ),
            _ => Task.CompletedTask
        };

        await action;
    }

    public async Task HandleErrorAsync(
        Exception exception,
        Update update,
        CancellationToken cancellationToken
    )
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        if (exception is ApiRequestException && exception.Message.Contains("can't parse entities"))
        {
            await _botClient.SendTextMessageAsync(
                update.Message!.Chat.Id,
                "У меня не получилось отправить тебе текст дайджеста из-за ошибки в HTML-разметке. "
                    + "Отредактируй дайджест командой /edit и убедись, что все теги закрыты.",
                cancellationToken: cancellationToken
            );
        }

        _logger.LogError("HandleError: {ErrorMessage}", errorMessage);
    }

    private static Task UnknownUpdateHandlerAsync(
        Update update,
        CancellationToken cancellationToken
    )
    {
        return Task.CompletedTask;
    }

    private static bool MessageIsText(Message message)
    {
        return message.Text is not null;
    }

    private static bool MessageIsForwarded(Message message)
    {
        return message.ForwardFrom is not null
            || message.ForwardFromChat is not null
            || message.ForwardFromMessageId is not null
            || message.ForwardDate is not null;
    }

    private static bool MessageIsFromPrivateChat(Message message)
    {
        return message.Chat.Type == ChatType.Private;
    }
}