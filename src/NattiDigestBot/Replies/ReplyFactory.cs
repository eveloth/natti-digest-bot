using System.Text;
using NattiDigestBot.Domain;
using Telegram.Bot.Types.Enums;

namespace NattiDigestBot.Replies;

public static class ReplyFactory
{
    public static Reply CategoriesListReply(IEnumerable<Category> categories)
    {
        var sb = new StringBuilder("Список твоих категорий:\n\n");

        var orderedCategories = categories.OrderBy(x => x.DisplayOrder == 0);

        foreach (var category in orderedCategories)
        {
            sb.Append($"\u2733 ({category.CategoryId}) ");
            sb.Append($"<b>{category.Keyword}</b> \u2733");
            sb.Append('\n');
            sb.Append($"Описание: {category.Description}");
            sb.Append('\n');
            sb.Append($"Порядок отображения: {category.DisplayOrder}");
            sb.Append('\n');
            sb.Append('\n');
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

        var entries = digest.DigestEntries
            .GroupBy(c => new { c.Category.CategoryId, c.Category.DisplayOrder })
            .OrderBy(x => x.Key.DisplayOrder == 0);

        foreach (var entryGroup in entries)
        {
            var category = entryGroup.First().Category;
            sb.Append($"<b>{category.Description}</b>:\n\n");

            foreach (var entry in entryGroup)
            {
                sb.Append($"\u2733 {entry.Description} - {entry.MessageLink}\n");
            }

            sb.Append('\n');
        }

        sb.Append("#дайджест #digest");

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

        var entries = digest.DigestEntries
            .GroupBy(c => new { c.Category.CategoryId, c.Category.DisplayOrder })
            .OrderBy(x => x.Key.DisplayOrder == 0);

        foreach (var entryGroup in entries)
        {
            var category = entryGroup.First().Category;
            sb.Append($"<b>{category.Keyword}</b>:\n\n");

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