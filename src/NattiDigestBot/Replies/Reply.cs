using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace NattiDigestBot.Replies;

public record Reply
{
    public string ReplyText { get; set; } = default!;
    public IReplyMarkup? ReplyMarkup { get; set; }
    public ParseMode? ParseMode { get; set; }
}