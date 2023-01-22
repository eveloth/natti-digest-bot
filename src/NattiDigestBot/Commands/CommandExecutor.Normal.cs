using NattiDigestBot.Domain;
using NattiDigestBot.Extensions;
using NattiDigestBot.Replies;
using NattiDigestBot.State;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

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

        var account = await _accountService.Get(userId, cancellationToken);

        if (account!.GroupId is not null)
        {
            await _botClient.SendReply(
                userId,
                NormalReplies.GroupAlreadyBoundReply,
                cancellationToken
            );
            return;
        }

        var argument = message.GetCommandArguments();

        if (argument is null)
        {
            await _botClient.SendReply(userId, NormalReplies.NoGroupIdReply, cancellationToken);
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

        var arguments = message.GetCommandArguments();

        if (arguments is null)
        {
            await _botClient.SendReply(
                userId,
                NormalReplies.NoCategoryArgumentReply,
                cancellationToken
            );
            return;
        }

        var categoryArguments = arguments.Split(" - ");

        if (
            categoryArguments.ElementAtOrDefault(0) is not { } keyword
            || categoryArguments.ElementAtOrDefault(1) is not { } description
        )
        {
            await _botClient.SendReply(
                userId,
                NormalReplies.InvalidCategoryArgumentReply,
                cancellationToken
            );
            return;
        }

        var categoryDisplayOrderArgument = categoryArguments.ElementAtOrDefault(2);

        var displayOrder = 0;

        if (
            categoryDisplayOrderArgument is not null
            && !int.TryParse(categoryDisplayOrderArgument, out displayOrder)
        )
        {
            await _botClient.SendReply(
                userId,
                NormalReplies.InvalidCategoryDisplayOrder,
                cancellationToken
            );
            return;
        }

        var newCategory = new Category
        {
            AccountId = userId,
            Keyword = keyword.EscpapeHtmlTagClosures(),
            Description = description.EscpapeHtmlTagClosures(),
            DisplayOrder = displayOrder
        };

        var result = await _categoryService.Create(newCategory, cancellationToken);

        if (!result)
        {
            await _botClient.SendReply(
                userId,
                NormalReplies.CategoryKeywordTakenReply,
                cancellationToken
            );
            return;
        }

        await _botClient.SendReply(userId, NormalReplies.CategoryCreatedRelpy, cancellationToken);
    }

    public async Task ShowCategories(Message message, CancellationToken cancellationToken)
    {
        var userId = message.Chat.Id;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(ShowCategories),
            userId
        );

        var categories = await _categoryService.GetAll(userId, cancellationToken);
        var reply = ReplyFactory.CategoriesListReply(categories);

        await _botClient.SendReply(userId, reply, cancellationToken);
    }

    public async Task UpdateCategory(Message message, CancellationToken cancellationToken)
    {
        var userId = message.Chat.Id;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(UpdateCategory),
            userId
        );

        var arguments = message.GetCommandArguments();

        if (arguments is null)
        {
            await _botClient.SendReply(
                userId,
                NormalReplies.NoCategoryArgumentReply,
                cancellationToken
            );
            return;
        }

        var categoryArguments = arguments.Split(" - ");

        var parsedSuccessfuly = int.TryParse(
            categoryArguments.ElementAtOrDefault(0),
            out var categoryId
        );

        if (
            !parsedSuccessfuly
            || categoryArguments.ElementAtOrDefault(1) is not { } keyword
            || categoryArguments.ElementAtOrDefault(2) is not { } description
        )
        {
            await _botClient.SendReply(
                userId,
                NormalReplies.InvalidCategoryArgumentReply,
                cancellationToken
            );
            return;
        }

        var categoryDisplayOrderArgument = categoryArguments.ElementAtOrDefault(3);

        var displayOrder = 0;

        if (
            categoryDisplayOrderArgument is not null
            && !int.TryParse(categoryDisplayOrderArgument, out displayOrder)
        )
        {
            await _botClient.SendReply(
                userId,
                NormalReplies.InvalidCategoryDisplayOrder,
                cancellationToken
            );
            return;
        }

        var updatedCategory = new Category
        {
            AccountId = userId,
            CategoryId = categoryId,
            Keyword = keyword.EscpapeHtmlTagClosures(),
            Description = description.EscpapeHtmlTagClosures(),
            DisplayOrder = displayOrder
        };

        var result = await _categoryService.Update(updatedCategory, cancellationToken);

        if (!result)
        {
            await _botClient.SendReply(
                userId,
                NormalReplies.CategoryNotFoundReply,
                cancellationToken
            );
            return;
        }

        await _botClient.SendReply(userId, NormalReplies.CategoryUpdatedRelpy, cancellationToken);
    }

    public async Task DeleteCategory(Message message, CancellationToken cancellationToken)
    {
        var userId = message.Chat.Id;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(DeleteCategory),
            userId
        );

        var argument = message.GetCommandArguments();

        var parsedSuccessfuly = int.TryParse(argument, out var categoryId);

        if (!parsedSuccessfuly)
        {
            await _botClient.SendReply(
                userId,
                NormalReplies.CategoryNotFoundReply,
                cancellationToken
            );
            return;
        }

        var result = await _categoryService.Delete(categoryId, userId, cancellationToken);

        if (!result)
        {
            await _botClient.SendReply(
                userId,
                NormalReplies.CategoryNotFoundReply,
                cancellationToken
            );
            return;
        }

        await _botClient.SendReply(userId, NormalReplies.CategoryDeletedRelpy, cancellationToken);
    }

    public async Task Digest(Message message, CancellationToken cancellationToken)
    {
        var userId = message.Chat.Id;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(Digest),
            userId
        );

        var argument = message.GetCommandArguments();

        if (argument is null)
        {
            await _botClient.SendReply(
                userId,
                NormalReplies.NoDigestArgumentReply,
                cancellationToken
            );
            return;
        }

        var parsedSuccessfuly = DateOnly.TryParse(argument, out var digestDate);

        if (!parsedSuccessfuly)
        {
            await _botClient.SendReply(
                userId,
                NormalReplies.InvalidDigestArgumentReply,
                cancellationToken
            );
            return;
        }

        var digest = await _digestService.Get(userId, digestDate, cancellationToken);

        if (digest is null)
        {
            digest = new Digest { AccountId = userId, Date = digestDate };
            await _digestService.Create(digest, cancellationToken);
        }

        StateStorage.SetChatModeFor(userId, ChatMode.Digest);
        StateStorage.SetCurrentDigest(userId, digest.Date);

        await _botClient.SendReply(
            userId,
            NormalReplies.EnteringDigestModeReply,
            cancellationToken
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

        var argument = message.GetCommandArguments();

        if (argument is null)
        {
            await _botClient.SendReply(
                userId,
                NormalReplies.NoDigestArgumentReply,
                cancellationToken
            );
            return;
        }

        var parsedSuccessfuly = DateOnly.TryParse(argument, out var digestDate);

        if (!parsedSuccessfuly)
        {
            await _botClient.SendReply(
                userId,
                NormalReplies.InvalidDigestArgumentReply,
                cancellationToken
            );
            return;
        }

        var digest = await _digestService.Get(userId, digestDate, cancellationToken);

        if (digest is null)
        {
            await _botClient.SendReply(
                userId,
                NormalReplies.DigestNotFoundReply,
                cancellationToken
            );
            return;
        }

        if (digest.DigestText is null)
        {
            await _botClient.SendReply(userId, NormalReplies.DigestNotMadeReply, cancellationToken);
            return;
        }

        //var reply = ReplyFactory.DigestPreviewReply(digest, ParseMode.Html);
        var reply = new Reply { ReplyText = digest.DigestText, ParseMode = ParseMode.Html };
        await _botClient.SendReply(userId, reply, cancellationToken);
    }

    public async Task Edit(Message message, CancellationToken cancellationToken)
    {
        var userId = message.Chat.Id;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(Edit),
            userId
        );

        var argument = message.GetCommandArguments();

        if (argument is null)
        {
            await _botClient.SendReply(
                userId,
                NormalReplies.NoDigestArgumentReply,
                cancellationToken
            );
            return;
        }

        var parsedSuccessfuly = DateOnly.TryParse(argument, out var digestDate);

        if (!parsedSuccessfuly)
        {
            await _botClient.SendReply(
                userId,
                NormalReplies.InvalidDigestArgumentReply,
                cancellationToken
            );
            return;
        }

        var digest = await _digestService.Get(userId, digestDate, cancellationToken);

        if (digest is null)
        {
            await _botClient.SendReply(
                userId,
                NormalReplies.DigestNotFoundReply,
                cancellationToken
            );
            return;
        }

        if (digest.DigestText is null)
        {
            await _botClient.SendReply(userId, NormalReplies.DigestNotMadeReply, cancellationToken);
            return;
        }

        StateStorage.SetChatModeFor(userId, ChatMode.Edit);
        StateStorage.SetCurrentDigest(userId, digest.Date);

        await _botClient.SendReply(userId, NormalReplies.EnteringEditModeReply, cancellationToken);

        var reply = new Reply { ReplyText = digest.DigestText };
        await _botClient.SendReply(userId, reply, cancellationToken);
    }

    public async Task Send(Message message, CancellationToken cancellationToken)
    {
        var userId = message.Chat.Id;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(Send),
            userId
        );

        var argument = message.GetCommandArguments();

        if (argument is null)
        {
            await _botClient.SendReply(
                userId,
                NormalReplies.NoDigestArgumentReply,
                cancellationToken
            );
            return;
        }

        var parsedSuccessfuly = DateOnly.TryParse(argument, out var digestDate);

        if (!parsedSuccessfuly)
        {
            await _botClient.SendReply(
                userId,
                NormalReplies.InvalidDigestArgumentReply,
                cancellationToken
            );
            return;
        }

        var digest = await _digestService.Get(userId, digestDate, cancellationToken);

        if (digest is null)
        {
            await _botClient.SendReply(
                userId,
                NormalReplies.DigestNotFoundReply,
                cancellationToken
            );
            return;
        }

        if (digest.IsSent)
        {
            await _botClient.SendReply(
                userId,
                NormalReplies.DigestAlreadySentReply,
                cancellationToken
            );
            return;
        }

        var account = await _accountService.Get(userId, cancellationToken);

        var groupId = account!.GroupId;

        if (groupId is null)
        {
            await _botClient.SendReply(userId, NormalReplies.GroupIdNotSetReply, cancellationToken);
            return;
        }

        var digestText = digest.DigestText;

        if (digestText is null)
        {
            await _botClient.SendReply(userId, NormalReplies.DigestNotMadeReply, cancellationToken);
            return;
        }

        await _botClient.SendReply(userId, NormalReplies.DigestSentReply, cancellationToken);

        var sentMessage = await _botClient.SendTextMessageAsync(
            groupId,
            digestText,
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken
        );
        var sentDigestMessageId = sentMessage.MessageId;

        if (account.PinnedDigestMessageId is not null)
        {
            await _botClient.UnpinChatMessageAsync(
                groupId,
                account.PinnedDigestMessageId,
                cancellationToken: cancellationToken
            );
        }

        await _botClient.PinChatMessageAsync(
            groupId,
            sentDigestMessageId,
            cancellationToken: cancellationToken
        );

        digest.IsSent = true;
        account.PinnedDigestMessageId = sentDigestMessageId;

        await _digestService.Update(digest, cancellationToken);
        await _accountService.SetPinnedDigest(userId, sentDigestMessageId, cancellationToken);
    }
}