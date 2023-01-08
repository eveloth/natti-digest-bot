using System.Diagnostics;
using NattiDigestBot.State;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace NattiDigestBot.Commands;

public partial class CommandExecutor
{
    public async Task Start(Message message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Executing command {Command}", nameof(Start));

        const string reply =
            "Привет! Я — бот-помощник, помогаю создавать дайджесты для больших групп.\n\n"
            + "Среди тысяч сообщений может затеряться что-нибудь интересное, а чтобы этого не произошло, "
            + "ты можешь воспользоваться моей помощью.\n\n"
            + "Для того, чтобы начать пользоваться ботом, тебе нужно привязать группу, которую ты администрируешь, "
            + "подтвердить, что это действительно ты — чтобы никто не мог отправлять в группы спам "
            + "с помощью дайджестов, и добавить хотя бы одну категорию.\n\n"
            + "Чтобы узнать подробную информацию о каждой команде, напиши /help.";

        var userId = message.Chat.Id;

        await _botClient.SendTextMessageAsync(userId, reply, cancellationToken: cancellationToken);
    }

    public async Task Bind(Message message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Executing command {Command}", nameof(Bind));

        var userId = message.Chat.Id;
        var argument = message.Text!.Split(' ', 2).ElementAtOrDefault(1);

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
        _logger.LogInformation("Executing command {Command}", nameof(Unbind));

        var userId = message.Chat.Id;
        await _accountService.UnbindGroup(userId, cancellationToken);

        const string reply = "Теперь группа отвязана от твоего аккаунта.";
        await _botClient.SendTextMessageAsync(userId, reply, cancellationToken: cancellationToken);
    }

    public async Task Confirm(Message message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Executing command {Command}", nameof(Confirm));

        var userId = message.Chat.Id;
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
        }

        StateStorage.AddToWaitingForConfirmationList(account.GroupId.Value, account.AccountId);

        const string reply = "Жду от тебя подтверждения в группе!";
        await _botClient.SendTextMessageAsync(userId, reply, cancellationToken: cancellationToken);
    }

    public Task Digest(Message message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Executing command {Command}", nameof(Digest));

        throw new NotImplementedException();
    }
}