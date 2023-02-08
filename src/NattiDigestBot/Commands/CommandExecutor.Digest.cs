using NattiDigestBot.Commands.Interfaces;
using NattiDigestBot.Domain;
using NattiDigestBot.Extensions;
using NattiDigestBot.Replies;
using NattiDigestBot.State;
using Telegram.Bot.Types;

namespace NattiDigestBot.Commands;

public partial class CommandExecutor : ICommandExecutor
{
    public async Task AddEntry(Message message, CancellationToken cancellationToken)
    {
        var userId = message.Chat.Id;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(AddEntry),
            userId
        );

        var parsedEntry = message.Text!.Split('\n');

        var categoryKeyword = parsedEntry.ElementAtOrDefault(0);
        var entryDescrition = parsedEntry.ElementAtOrDefault(1);
        var messageLink = parsedEntry.ElementAtOrDefault(2);

        if (categoryKeyword is null || entryDescrition is null || messageLink is null)
        {
            await _botClient.SendReply(
                userId,
                DigestReplies.ErrorParsingEntryReply,
                cancellationToken
            );
            return;
        }

        var category = await _categoryService.GetByKeyword(
            userId,
            categoryKeyword,
            cancellationToken
        );

        if (category is null)
        {
            await _botClient.SendReply(
                userId,
                DigestReplies.CatergoryNotFoundReply,
                cancellationToken
            );
            return;
        }

        var digestDate = StateStorage.GetCurrentDigestDate(userId);

        var entry = new DigestEntry
        {
            DigestId = userId,
            Date = digestDate,
            Category = category,
            Description = entryDescrition,
            MessageLink = messageLink
        };

        await _digestService.AddEntry(entry, cancellationToken);
        await _botClient.SendReply(userId, DigestReplies.EntryAddedReply, cancellationToken);
    }

    public async Task UpdateEntry(Message message, CancellationToken cancellationToken)
    {
        var userId = message.Chat.Id;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(UpdateEntry),
            userId
        );

        var firstLine = message.Text!.Split('\n')[0];
        var argument = firstLine.Split(' ').ElementAtOrDefault(1);

        if (argument is null)
        {
            await _botClient.SendReply(
                userId,
                DigestReplies.NoEntryArgumentReply,
                cancellationToken
            );
            return;
        }

        var parsedSuccessfuly = int.TryParse(argument, out var entryId);

        if (!parsedSuccessfuly)
        {
            await _botClient.SendReply(userId, DigestReplies.EntryNotFoundReply, cancellationToken);
            return;
        }

        var digestDate = StateStorage.GetCurrentDigestDate(userId);

        var entry = await _digestService.GetEntry(userId, digestDate, entryId, cancellationToken);

        if (entry is null)
        {
            await _botClient.SendReply(userId, DigestReplies.EntryNotFoundReply, cancellationToken);
            return;
        }

        var parsedEntry = message.Text!.Split('\n');

        if (parsedEntry.Any(string.IsNullOrEmpty))
        {
            await _botClient.SendReply(
                userId,
                DigestReplies.ErrorParsingEntryReply,
                cancellationToken
            );
            return;
        }

        var categoryKeyword = parsedEntry[1];
        var entryDescrition = parsedEntry[2];
        var messageLink = parsedEntry[3];

        var category = await _categoryService.GetByKeyword(
            userId,
            categoryKeyword,
            cancellationToken
        );

        if (category is null)
        {
            await _botClient.SendReply(
                userId,
                DigestReplies.CatergoryNotFoundReply,
                cancellationToken
            );
            return;
        }

        entry.Category = category;
        entry.Description = entryDescrition;
        entry.MessageLink = messageLink;

        await _digestService.UpdateEntry(entry, cancellationToken);
        await _botClient.SendReply(userId, DigestReplies.EntryUpdatedReply, cancellationToken);
    }

    public async Task RemoveEntry(Message message, CancellationToken cancellationToken)
    {
        var userId = message.Chat.Id;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(RemoveEntry),
            userId
        );

        var argument = message.GetCommandArguments();

        if (argument is null)
        {
            await _botClient.SendReply(
                userId,
                DigestReplies.NoEntryArgumentReply,
                cancellationToken
            );
            return;
        }

        var parsedSuccessfuly = int.TryParse(argument, out var entryId);

        if (!parsedSuccessfuly)
        {
            await _botClient.SendReply(userId, DigestReplies.EntryNotFoundReply, cancellationToken);
            return;
        }

        var digestDate = StateStorage.GetCurrentDigestDate(userId);

        var entry = await _digestService.GetEntry(userId, digestDate, entryId, cancellationToken);

        if (entry is null)
        {
            await _botClient.SendReply(userId, DigestReplies.EntryNotFoundReply, cancellationToken);
            return;
        }

        await _digestService.DeleteEntry(entry, cancellationToken);
        await _botClient.SendReply(userId, DigestReplies.EntryDeletedReply, cancellationToken);
    }

    public async Task RawPreview(Message message, CancellationToken cancellationToken)
    {
        var userId = message.Chat.Id;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(RawPreview),
            userId
        );

        var digestDate = StateStorage.GetCurrentDigestDate(userId);
        var digest = await _digestService.Get(userId, digestDate, cancellationToken);

        var reply = ReplyFactory.RawPreviewReply(digest!);
        await _botClient.SendReply(userId, reply, cancellationToken);
    }

    public async Task Make(Message message, CancellationToken cancellationToken)
    {
        var userId = message.Chat.Id;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(Make),
            userId
        );

        var digestDate = StateStorage.GetCurrentDigestDate(userId);
        var digest = await _digestService.Get(userId, digestDate, cancellationToken);

        var digestText = ReplyFactory.DigestPreviewReply(digest!).ReplyText;
        digest!.DigestText = digestText;

        await _digestService.Update(digest, cancellationToken);
        await _botClient.SendReply(userId, DigestReplies.DigestMadeReply, cancellationToken);
    }
}