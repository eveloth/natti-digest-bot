using NattiDigestBot.Extensions;
using NattiDigestBot.Replies;
using NattiDigestBot.State;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace NattiDigestBot.Commands;

public partial class CommandExecutor
{
    public async Task Start(Message message, CancellationToken cancellationToken)
    {
        var userId = message.Chat.Id;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(Start),
            userId
        );

        await _botClient.SendReply(userId, GeneralReplies.StartReply, cancellationToken);
    }

    public async Task Bind(Message message, CancellationToken cancellationToken)
    {
        var userId = message.Chat.Id;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(Bind),
            userId
        );

        var argument = message.GetCommandArguments();

        if (argument is null)
        {
            const string errorReply = "Пожалуйста, укажи Id твоей группы после команды /bind.";
            await _botClient.SendTextMessageAsync(
                userId,
                errorReply,
                cancellationToken: cancellationToken
            );
            return;
        }

        var parsedSuccessfuly = long.TryParse(argument, out var groupId);

        if (!parsedSuccessfuly)
        {
            const string parsingErrorReply =
                "Ой, не могу прочесть Id группы. Пожалуйста, проверь, что Id правильный!";

            await _botClient.SendTextMessageAsync(
                userId,
                parsingErrorReply,
                cancellationToken: cancellationToken
            );
            return;
        }

        //TODO: add group id validation

        await _accountService.BindGroup(userId, groupId, cancellationToken);

        var reply =
            "Отлично! Группа привязана, осталось подтвердить, что ты её администратор."
            + $"Введи команду /confirm, а потом введи её же в группе вот так: /confirm@{StateStorage.BotName}.\n\n"
            + "Чтобы узнать подробнее, что нужно делать, введи команду /help и посмотри инструкицию "
            + "для команды /confirm.";
        await _botClient.SendTextMessageAsync(userId, reply, cancellationToken: cancellationToken);
    }

    public async Task Unbind(Message message, CancellationToken cancellationToken)
    {
        var userId = message.Chat.Id;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(Unbind),
            userId
        );

        await _accountService.UnbindGroup(userId, cancellationToken);

        const string reply = "Теперь группа отвязана от твоего аккаунта.";
        await _botClient.SendTextMessageAsync(userId, reply, cancellationToken: cancellationToken);
    }

    public async Task StartConfirmationProcess(Message message, CancellationToken cancellationToken)
    {
        var userId = message.Chat.Id;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(StartConfirmationProcess),
            userId
        );

        var account = await _accountService.Get(userId, cancellationToken);

        if (account!.GroupId is null)
        {
            const string noBoundGroupReply =
                "Кажется, к твоему аккаунту ещё не привязана группа. Привяжи её с помощью "
                + "команды /bind, и после этого ты сможешь подтвердить, что ты её администратор.";

            await _botClient.SendTextMessageAsync(
                userId,
                noBoundGroupReply,
                cancellationToken: cancellationToken
            );
            return;
        }

        if (account.IsGroupConfirmed)
        {
            const string alreadyConfirmedReply =
                "Группа уже подтверждена, можешь отправлять в неё дайджесты!";

            await _botClient.SendTextMessageAsync(
                userId,
                alreadyConfirmedReply,
                cancellationToken: cancellationToken
            );

            return;
        }

        StateStorage.AddToWaitingForConfirmationList(account.GroupId.Value, account.AccountId);

        const string reply = "Жду от тебя подтверждения в группе!";
        await _botClient.SendTextMessageAsync(userId, reply, cancellationToken: cancellationToken);
    }

    public Task Digest(Message message, CancellationToken cancellationToken)
    {
        var userId = message.Chat.Id;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(Digest),
            userId
        );

        throw new NotImplementedException();
    }

    public Task Preview(Message message, CancellationToken cancellationToken)
    {
        var userId = message.Chat.Id;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(Preview),
            userId
        );
        throw new NotImplementedException();
    }

    public Task Edit(Message message, CancellationToken cancellationToken)
    {
        var userId = message.Chat.Id;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(Edit),
            userId
        );
        throw new NotImplementedException();
    }

    public Task Send(Message message, CancellationToken cancellationToken)
    {
        var userId = message.Chat.Id;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(Send),
            userId
        );
        throw new NotImplementedException();
    }
}