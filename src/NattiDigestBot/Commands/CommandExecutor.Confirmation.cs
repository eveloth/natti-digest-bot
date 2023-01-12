using NattiDigestBot.Extensions;
using NattiDigestBot.Replies;
using NattiDigestBot.State;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace NattiDigestBot.Commands;

public partial class CommandExecutor
{
    public async Task ConfirmGroup(Message message, CancellationToken cancellationToken)
    {
        var groupId = message.Chat.Id;

        _logger.LogInformation("Attempting to confirm group ID {GroupId}", groupId);

        var isAdmin = await SenderIsAdmin(message, cancellationToken);

        if (!isAdmin)
        {
            await _botClient.DeleteMessageAsync(
                groupId,
                message.MessageId,
                cancellationToken: cancellationToken
            );
            return;
        }

        await _botClient.DeleteMessageAsync(
            groupId,
            message.MessageId,
            cancellationToken: cancellationToken
        );

        var userId = StateStorage.GetGroupOwner(groupId);

        await _accountService.ConfirmGroupForAccount(userId, cancellationToken);

        StateStorage.RemoveFromWaitingForConfirmationList(groupId);
        StateStorage.SetChatModeFor(userId, ChatMode.Normal);

        await _botClient.SendReply(
            userId,
            ConfirmationReplies.GroupConfirmedReply,
            cancellationToken
        );
    }

    public async Task HandleMessageInConfirmationMode(
        Message message,
        CancellationToken cancellationToken
    )
    {
        var userId = message.Chat.Id;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(HandleMessageInConfirmationMode),
            userId
        );

        await _botClient.SendReply(
            userId,
            ConfirmationReplies.InteractiveModeAnnouncement,
            cancellationToken
        );
    }

    private async Task<bool> SenderIsAdmin(Message message, CancellationToken cancellationToken)
    {
        var sender = message.From!;
        var groupId = message.Chat.Id;

        var user = await _botClient.GetChatMemberAsync(groupId, sender.Id, cancellationToken);

        return user.Status is ChatMemberStatus.Administrator or ChatMemberStatus.Creator;
    }
}