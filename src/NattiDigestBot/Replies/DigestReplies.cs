using Telegram.Bot.Types.Enums;

namespace NattiDigestBot.Replies;

public static class DigestReplies
{
    public static Reply EntryAddedReply { get; } =
        new() { ReplyText = "Сообщение добавлено в дайджест!" };

    public static Reply ErrorParsingEntryReply { get; } =
        new()
        {
            ReplyText =
                "Не удалось добавить сообщение в дайджест. "
                + "Убедись, что формат правильный:\n"
                + "<i>категория\nописание\nссылка</i>",
            ParseMode = ParseMode.Html
        };

    public static Reply CatergoryNotFoundReply { get; } =
        new()
        {
            ReplyText =
                "Не удалось найти категорию по ключевому слову. "
                + "Проверь, что ты вводишь её правильно с помощью команды /categories."
        };

    public static Reply NoEntryArgumentReply { get; } =
        new() { ReplyText = "Пожалуйста, укажи ID сообщения." };

    public static Reply EntryDeletedReply { get; } =
        new() { ReplyText = "Сообщение удалено из дайджеста!" };

    public static Reply EntryUpdatedReply { get; } =
        new() { ReplyText = "Сообщение обновлено!" };

    public static Reply EntryNotFoundReply { get; } =
        new()
        {
            ReplyText =
                "Не получилось найти сообщение. "
                + "Убедись, что его ID указан правильно, посмотреть ID можно "
                + "с помощью команды /raw_preview."
        };

    public static Reply DigestMadeReply { get; } =
        new()
        {
            ReplyText =
                "Дайджест сформирован! Теперь ты можешь выйти из интерактивного режима "
                + "с помощью команды /exit, отредактировать текст дайжеста "
                + "с помощью команды /edit и отправить командой /send. Не забудь после команд указать дату дайджеста."
        };
}