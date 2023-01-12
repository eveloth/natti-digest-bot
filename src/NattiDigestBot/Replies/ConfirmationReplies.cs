namespace NattiDigestBot.Replies;

public static class ConfirmationReplies
{
    public static Reply GroupConfirmedReply { get; } =
        new()
        {
            ReplyText = "Ура, группа подтверждена! Теперь ты можешь отправлять в неё дайджесты."
        };

    public static Reply InteractiveModeAnnouncement { get; } =
        new()
        {
            ReplyText =
                "Сейчас я жду от тебя подтверждения в группе. Если хочешь отменить подтверждение, напиши /exit."
        };
}