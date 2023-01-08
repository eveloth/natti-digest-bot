using NattiDigestBot.State;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace NattiDigestBot.Commands;

public partial class CommandExecutor
{
    public async Task ConfirmGroup(Message message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Executing command {Command}", nameof(ConfirmGroup));

        var isAdmin = await SenderIsAdmin(message, cancellationToken);

        if (!isAdmin)
        {
            await _botClient.DeleteMessageAsync(
                message.Chat.Id,
                message.MessageId,
                cancellationToken: cancellationToken
            );
            return;
        }

        var groupId = message.Chat.Id;
        var userId = StateStorage.GetGroupOwner(groupId);

        await _accountService.ConfirmGroupForAccount(userId, cancellationToken);

        StateStorage.RemoveFromWaitingForConfirmationList(groupId);
        StateStorage.SetChatModeFor(userId, ChatMode.Normal);

        const string reply =
            "Ура, группа подтверждена! Теперь ты можешь отправлять в неё дайджесты.";
        await _botClient.SendTextMessageAsync(userId, reply, cancellationToken: cancellationToken);
    }

    public async Task HandleMessageInConfirmationMode(
        Message message,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(
            "Executing command {Command}",
            nameof(HandleMessageInConfirmationMode)
        );

        var userId = message.Chat.Id;
        const string reply =
            "Сейчас я жду от тебя подтверждения в группе. Если хочешь отменить подтверждение, напиши /exit.";
        await _botClient.SendTextMessageAsync(userId, reply, cancellationToken: cancellationToken);
    }

    private async Task<bool> SenderIsAdmin(Message message, CancellationToken cancellationToken)
    {
        var sender = message.From!;
        var groupId = message.Chat.Id;

        var user = await _botClient.GetChatMemberAsync(groupId, sender.Id, cancellationToken);

        return user.Status is ChatMemberStatus.Administrator or ChatMemberStatus.Creator;
    }
}