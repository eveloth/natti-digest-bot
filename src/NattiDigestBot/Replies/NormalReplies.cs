using NattiDigestBot.State;

namespace NattiDigestBot.Replies;

public static class NormalReplies
{
    public static Reply NoGroupIdErrorReply { get; } =
        new() { ReplyText = "Пожалуйста, укажи Id твоей группы после команды /bind." };

    public static Reply GroupIdParsingErrorReply { get; } =
        new()
        {
            ReplyText = "Ой, не могу прочесть ID группы. Пожалуйста, проверь, что ID правильный!"
        };

    public static Reply GroupBoundReply { get; } =
        new()
        {
            ReplyText =
                "Отлично! Группа привязана, осталось подтвердить, что ты её администратор."
                + "Введи команду /confirm, а потом введи её же в группе вот так: "
                + $"/confirm@{StateStorage.BotName}.\n\n"
        };

    public static Reply GroupUnboundReply { get; } =
        new() { ReplyText = "Теперь группа отвязана от твоего аккаунта." };

    public static Reply GroupIdNotSetReply { get; } =
        new()
        {
            ReplyText =
                "Кажется, к твоему аккаунту ещё не привязана группа. Привяжи её с помощью "
                + "команды /bind, и после этого ты сможешь подтвердить, что ты её администратор."
        };

    public static Reply GroupAlreadyConfirmedReply { get; } =
        new() { ReplyText = "Группа уже подтверждена, можешь отправлять в неё дайджесты!" };

    public static Reply WaitingForConfirmationReply { get; } =
        new() { ReplyText = "Жду от тебя подтверждения в группе!" };
}