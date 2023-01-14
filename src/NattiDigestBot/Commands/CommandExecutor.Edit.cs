using NattiDigestBot.Commands.Interfaces;
using NattiDigestBot.Extensions;
using NattiDigestBot.Replies;
using NattiDigestBot.State;
using Telegram.Bot.Types;

namespace NattiDigestBot.Commands;

public partial class CommandExecutor : ICommandExecutor
{
    public async Task UpdateDigestText(Message message, CancellationToken cancellationToken)
    {
        var userId = message.Chat.Id;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(UpdateDigestText),
            userId
        );

        var digestDate = StateStorage.GetCurrentDigestDate(userId);
        var digest = await _digestService.Get(userId, digestDate, cancellationToken);

        digest!.DigestText = message.Text;
        await _digestService.Update(digest, cancellationToken);

        StateStorage.SetChatModeFor(userId, ChatMode.Normal);
        await _botClient.SendReply(userId, EditReplies.EditSuccessfulReply, cancellationToken);
    }
}