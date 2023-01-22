using NattiDigestBot.Replies.Menus;
using NattiDigestBot.State;
using Telegram.Bot.Types.Enums;

namespace NattiDigestBot.Replies;

public static class NormalReplies
{
    public static Reply NoGroupIdReply { get; } =
        new() { ReplyText = "Пожалуйста, укажи ID твоей группы после команды /bind." };

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
                "Кажется, к твоему аккаунту ещё не привязана группа. Привяжи её с помощью команды /bind."
        };

    public static Reply GroupAlreadyBoundReply { get; } =
        new()
        {
            ReplyText =
                "К твоему аккаунту уже привязана группа. Прежде чем привязать новую, "
                + "тебе нужно отвязать старую командой /unbind."
        };

    public static Reply GroupAlreadyConfirmedReply { get; } =
        new() { ReplyText = "Группа уже подтверждена, можешь отправлять в неё дайджесты!" };

    public static Reply WaitingForConfirmationReply { get; } =
        new() { ReplyText = "Жду от тебя подтверждения в группе!" };

    public static Reply NoCategoryArgumentReply { get; } =
        new() { ReplyText = "Укажи, пожалуйста, ключевое слово и описание категории." };

    public static Reply InvalidCategoryArgumentReply { get; } =
        new()
        {
            ReplyText =
                "Не получается прочесть ключевое слово и описание категории. Проверь, пожалуйста, "
                + "что они указаны в правильном формате: \n\n"
                + "/new_category <i>ключ - описание</i> — для новой категории,\n"
                + "/update_category <i>ID - ключ - описание</i> — для редактирования категории",
            ParseMode = ParseMode.Html
        };

    public static Reply CategoryNotFoundReply { get; } =
        new()
        {
            ReplyText = "Ой, что-то пошло не так: убедись, что ID категории указан правильно."
        };

    public static Reply CategoryKeywordTakenReply { get; } =
        new()
        {
            ReplyText =
                "Кажется, это ключевое слово уже занято. Пожалуйста, выбери другое и попробуй снова!"
        };

    public static Reply CategoryCreatedRelpy { get; } = new() { ReplyText = "Категория создана!" };

    public static Reply CategoryUpdatedRelpy { get; } =
        new() { ReplyText = "Категория обновлена!" };

    public static Reply CategoryDeletedRelpy { get; } = new() { ReplyText = "Категория удалена!" };

    public static Reply NoDigestArgumentReply { get; } =
        new() { ReplyText = "Укажи, пожалуйста, дату дайджеста!" };

    public static Reply InvalidDigestArgumentReply { get; } =
        new()
        {
            ReplyText =
                "У меня не получилось понять, дайджест за какую дату ты имеешь в виду. "
                + "Укажи, пожалуйста, дату вот в таком формате: <i>31/01/09</i>",
            ParseMode = ParseMode.Html
        };

    public static Reply EnteringDigestModeReply { get; } =
        new()
        {
            ReplyText =
                "Интерактивный режим создания и редактирования дайджеста включён! \n\n"
                + "Все сообщения, кроме команд /raw_preview, /categories, /delete и /make, "
                + "я постараюсь записать в дайджест, — "
                + "не забудь указать категорию, описание и ссылку. Чтобы выйти из этого режима, "
                + "дай команду /exit."
        };

    public static Reply DigestNotFoundReply { get; } =
        new() { ReplyText = "У меня не получилось найти этот дайджест :(" };

    public static Reply DigestNotMadeReply { get; } =
        new()
        {
            ReplyText =
                "Кажется, дайджест ещё не сформирован — перейди в режим создания дайджеста "
                + "и дай команду /make."
        };

    public static Reply DigestAlreadySentReply { get; } =
        new() { ReplyText = "Этот дайджест уже был отправлен." };

    public static Reply DigestSentReply { get; } = new() { ReplyText = "Дайджест отправлен!" };

    public static Reply EnteringEditModeReply { get; } =
        new()
        {
            ReplyText =
                "Финальные штрихи! Сейчас я пришлю тебе сообщение с текстом дайджеста, а ты сможешь "
                + "отредактировать его так, как тебе нравится — просто скопируй его, исправь то, "
                + "что нужно, и пришли мне обратно.\n\n"
                + "Для стилей, вроде жирного или подчёркнутого текста, спойлеров и всего такого "
                + "используй HTML.",
            ReplyMarkup = HtmlReferenceMenuMarkup.HtmlReference
        };
}