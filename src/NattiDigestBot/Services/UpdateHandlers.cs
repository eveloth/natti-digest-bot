using NattiDigestBot.Data.Models;
using NattiDigestBot.StateMachine;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace NattiDigestBot.Services;

public class UpdateHandlers
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<UpdateHandlers> _logger;
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IAccountService _accountService;

    public UpdateHandlers(
        ITelegramBotClient botClient,
        ILogger<UpdateHandlers> logger,
        ICommandDispatcher commandDispatcher, IAccountService accountService)
    {
        _botClient = botClient;
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

        if (message.Text is null)
            return;

        if (message.ForwardFrom is not null)
            return;

        if (message.Chat.Type != ChatType.Private)
        {
            if (!StateStorage.WaitingForConfirmation.ContainsKey(chatId))
            {
                return;
            }

            await _commandDispatcher.HandleWaitingForConfirmationMode(chatId, cancellationToken);
            return;

            var accountId = StateStorage.WaitingForConfirmation[chatId];
            await _accountService.ConfirmGroup(accountId, chatId, cancellationToken);
            StateStorage.WaitingForConfirmation.Remove(chatId);
            StateStorage.ChatMode[accountId] = ChatMode.Normal;
            return;
        }

        var account = await _accountService.Get(message.Chat.Id, cancellationToken);

        if (account is null)
        {
            var newAccount = new Account
            {
                AccountId = message.Chat.Id
            };

            await _accountService.Create(newAccount, cancellationToken);
            StateStorage.ChatMode.Add(newAccount.AccountId, ChatMode.Normal);
            await _commandDispatcher.HandleNormalMode(message, cancellationToken);
            return;
        }

        var currentMode = StateStorage.ChatMode[message.Chat.Id];

        var action = currentMode switch
        {
            ChatMode.Normal => _commandDispatcher.HandleNormalMode(message, cancellationToken),
            ChatMode.Digest => _commandDispatcher.HandleDigestMode(message, cancellationToken),
            ChatMode.Edit   => _commandDispatcher.HandleEditMode(message, cancellationToken),
            _               => Task.CompletedTask
        };

        await action;

        static async Task<Message> Usage(
            ITelegramBotClient botClient,
            Message message,
            CancellationToken cancellationToken
        )
        {
            const string usage = "команда\nкатегория\nописание\nссылка";

            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: usage,
                cancellationToken: cancellationToken
            );
        }
    }

    private Task UnknownUpdateHandlerAsync(Update update, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
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
}