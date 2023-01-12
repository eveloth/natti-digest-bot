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
            await _botClient.SendReply(
                userId,
                NormalReplies.NoGroupIdErrorReply,
                cancellationToken
            );
            return;
        }

        var parsedSuccessfuly = long.TryParse(argument, out var groupId);
        //Group id's can be negative numbers type of long but they have no exact length by contract
        //and bot is unable to tell if the group actually exists
        var supposedlyIsGroupId = groupId < 0;

        if (!parsedSuccessfuly || !supposedlyIsGroupId)
        {
            await _botClient.SendReply(
                userId,
                NormalReplies.GroupIdParsingErrorReply,
                cancellationToken
            );
            return;
        }

        await _accountService.BindGroup(userId, groupId, cancellationToken);

        await _botClient.SendReply(
            userId,
            NormalReplies.GroupBoundReply,
            cancellationToken: cancellationToken
        );
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

        await _botClient.SendReply(
            userId,
            NormalReplies.GroupUnboundReply,
            cancellationToken: cancellationToken
        );
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
            await _botClient.SendReply(userId, NormalReplies.GroupIdNotSetReply, cancellationToken);
            return;
        }

        if (account.IsGroupConfirmed)
        {
            await _botClient.SendReply(
                userId,
                NormalReplies.GroupAlreadyConfirmedReply,
                cancellationToken
            );
            return;
        }

        StateStorage.AddToWaitingForConfirmationList(account.GroupId.Value, account.AccountId);

        await _botClient.SendReply(
            userId,
            NormalReplies.WaitingForConfirmationReply,
            cancellationToken: cancellationToken
        );
    }

    public async Task NewCategory(Message message, CancellationToken cancellationToken)
    {
        var userId = message.Chat.Id;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(NewCategory),
            userId
        );
    }

    public async Task ShowCategories(Message message, CancellationToken cancellationToken)
    {
        var userId = message.Chat.Id;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(ShowCategories),
            userId
        );
    }

    public async Task EditCategory(Message message, CancellationToken cancellationToken)
    {
        var userId = message.Chat.Id;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(EditCategory),
            userId
        );
    }

    public async Task DeleteCategory(Message message, CancellationToken cancellationToken)
    {
        var userId = message.Chat.Id;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(DeleteCategory),
            userId
        );
    }

    public async Task Digest(Message message, CancellationToken cancellationToken)
    {
        var userId = message.Chat.Id;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(Digest),
            userId
        );
    }

    public async Task Preview(Message message, CancellationToken cancellationToken)
    {
        var userId = message.Chat.Id;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(Preview),
            userId
        );
    }

    public async Task Edit(Message message, CancellationToken cancellationToken)
    {
        var userId = message.Chat.Id;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(Edit),
            userId
        );
    }

    public async Task Send(Message message, CancellationToken cancellationToken)
    {
        var userId = message.Chat.Id;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(Send),
            userId
        );
    }
}