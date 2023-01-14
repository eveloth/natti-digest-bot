using System.Text;
using Microsoft.Extensions.Primitives;
using NattiDigestBot.Domain;
using Telegram.Bot.Types.Enums;

namespace NattiDigestBot.Replies;

public static class ReplyFactory
{
    public static Reply CategoriesListReply(IEnumerable<Category> categories)
    {
        var sb = new StringBuilder("Список твоих категорий:\n\n");

        foreach (var category in categories)
        {
            sb.Append($"\u2733 ({category.CategoryId}) ");
            sb.Append($"<b>{category.Keyword}</b>");
            sb.Append("\n");
            sb.Append(category.Description);
            sb.Append("\n");
        }

        var replyText = sb.ToString();

        return new Reply { ReplyText = replyText, ParseMode = ParseMode.Html };
    }

    public static Reply DigestPreviewReply(Digest digest, ParseMode? parseMode = null)
    {
        var sb = new StringBuilder($"<b>Дайджест от {digest.Date}</b>\n\n");

        sb.Append("Рассказываем, что сегодня было интересного!\n\n");

        if (digest.DigestEntries.Count == 0)
        {
            sb.Append("Пока что в этом дайджесте нет сообщений.");
            var emptyDigestReply = sb.ToString();

            return new Reply { ReplyText = emptyDigestReply, ParseMode = ParseMode.Html };
        }

        var entries = digest.DigestEntries.GroupBy(c => c.CategoryId);

        foreach (var entryGroup in entries)
        {
            var category = entryGroup.First().Category;
            sb.Append($"<b>{category.Description}</b>:\n");

            foreach (var entry in entryGroup)
            {
                sb.Append($"\u2733 {entry.Description} - {entry.MessageLink}\n");
            }

            sb.Append("\n");
        }

        var replyText = sb.ToString();

        return new Reply { ReplyText = replyText, ParseMode = parseMode };
    }

    public static Reply RawPreviewReply(Digest digest)
    {
        var sb = new StringBuilder($"<b>Дайджест от {digest.Date}</b>\n\n");

        if (digest.DigestEntries.Count == 0)
        {
            sb.Append("Пока что в этом дайджесте нет сообщений.");
            var emptyDigestReply = sb.ToString();

            return new Reply { ReplyText = emptyDigestReply, ParseMode = ParseMode.Html };
        }
        var entries = digest.DigestEntries.GroupBy(c => c.CategoryId);

        foreach (var entryGroup in entries)
        {
            var category = entryGroup.First().Category;
            sb.Append($"<b>{category.Keyword}</b>:\n");

            foreach (var entry in entryGroup)
            {
                sb.Append(
                    $"\u2733 ({entry.DigestEntryId}) {entry.Description} - {entry.MessageLink}\n"
                );
            }

            sb.Append('\n');
        }

        var replyText = sb.ToString();

        return new Reply { ReplyText = replyText, ParseMode = ParseMode.Html };
    }
}