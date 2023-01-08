using NattiDigestBot.Domain;
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

    public UpdateHandlers(
        ILogger<UpdateHandlers> logger,
        ICommandDispatcher commandDispatcher,
        IAccountService accountService
    )
    {
        _logger = logger;
        _commandDispatcher = commandDispatcher;
        _accountService = accountService;
    }

    public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
    {
        var handler = update switch
        {
            { Message: { } message } => BotOnMessageReceived(message, cancellationToken),
            _ => UnknownUpdateHandlerAsync(update, cancellationToken)
        };

        await handler;
    }

    private async Task BotOnMessageReceived(Message message, CancellationToken cancellationToken)
    {
        var chatId = message.Chat.Id;
        _logger.LogInformation("Received message from chat ID {CharId}", chatId);

        if (!MessageIsText(message))
            return;

        if (MessageIsForwarded(message))
        {
            _logger.LogDebug("Message was forwarded");
            return;
        }

        _logger.LogDebug("{Message}", message.Text!);

        if (!MessageIsFromPrivateChat(message))
        {
            _logger.LogDebug("Message was not from the private chat");

            if (!StateStorage.IsWaitingForConfirmation(chatId))
            {
                _logger.LogDebug("Group was not in the waiting list");
                return;
            }

            _logger.LogDebug("Trying to confirm group");

            await _commandDispatcher.ReceiveConfirmationFromGroup(message, cancellationToken);
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

    public Task HandleErrorAsync(Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        _logger.LogInformation("HandleError: {ErrorMessage}", errorMessage);
        return Task.CompletedTask;
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
        return message.ForwardFrom is not null;
    }

    private static bool MessageIsFromPrivateChat(Message message)
    {
        return message.Chat.Type == ChatType.Private;
    }
}