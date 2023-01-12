using System.Text;
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
}