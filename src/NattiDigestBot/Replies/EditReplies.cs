using Telegram.Bot.Types.Enums;

namespace NattiDigestBot.Replies;

public static class EditReplies
{
    public static Reply EditSuccessfulReply { get; } = new()
    {
        ReplyText = "Текст дайджеста обновлён! Чтобы посмотреть, что получилось, введи команду " +
                    "/preview <i>дата_дайджеста</i>.",
        ParseMode = ParseMode.Html
    };
}