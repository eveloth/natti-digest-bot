using Telegram.Bot.Types.Enums;

namespace NattiDigestBot.Replies;

public static class GeneralReplies
{
    public static Reply StartReply { get; } =
        new()
        {
            ReplyText =
                "Привет! Я — бот-помощник, помогаю создавать дайджесты для больших групп.\n\n"
                + "Среди тысяч сообщений может затеряться что-нибудь интересное, а чтобы этого не произошло, "
                + "ты можешь воспользоваться моей помощью.\n\n"
                + "Чтобы узнать подробную информацию обо мне, напиши /help.",
        };

    public static Reply UsageReply { get; } =
        new()
        {
            ReplyText =
                "Что-то на эльфийском, не могу прочесть...\n\n"
                + "Чтобы узнать, как пользоваться ботом, напиши /help.",
            ParseMode = ParseMode.Html
        };

    public static Reply ReturningToNormalMode { get; } =
        new() { ReplyText = "Теперь ты находишься в обычном режиме и можешь давать боту команды." };

    public static Reply AccountDeletedReply { get; } =
        new()
        {
            ReplyText =
                "Твой аккаунт удалён! Чтобы начать всё заново, просто напиши мне любое сообщение, "
                + "например, /start."
        };
}