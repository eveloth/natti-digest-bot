using Telegram.Bot.Types.ReplyMarkups;

namespace NattiDigestBot.Replies.Menus;

public static class HtmlReferenceMenuMarkup
{
    public static InlineKeyboardMarkup HtmlReference { get; } =
        new(
            new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        "HTML — это как? \U0001f914",
                        $"{CallbackData.Html}:{CallbackData.Reference}"
                    ),
                }
            }
        );

    public static InlineKeyboardMarkup HtmlReferenceBack { get; } =
        new(
            new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        "\u21a9 Назад",
                        $"{CallbackData.Html}:{CallbackData.Back}"
                    ),
                }
            }
        );
}