using Telegram.Bot.Types.ReplyMarkups;

namespace NattiDigestBot.Replies.Menus;

public static class HelpMenuMarkups
{
    public static InlineKeyboardMarkup EntrypointMarkup { get; } =
        new(
            new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        "Как узнать ID группы?",
                        $"{CallbackData.Main}:{CallbackData.GroupId}"
                    ),
                    InlineKeyboardButton.WithCallbackData(
                        "Как привязать группу?",
                        $"{CallbackData.Main}:{CallbackData.Bind}"
                    ),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        "Про категории",
                        $"{CallbackData.Main}:{CallbackData.Categories}"
                    ),
                    InlineKeyboardButton.WithCallbackData(
                        "Про дайджесты",
                        $"{CallbackData.Main}:{CallbackData.Digest}"
                    ),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        "ID моего акканута",
                        $"{CallbackData.Main}:{CallbackData.AccountId}"
                    ),
                    InlineKeyboardButton.WithCallbackData(
                        "\u274c Удалить аккаунт",
                        $"{CallbackData.Main}:{CallbackData.DeleteAccountPropmt}"
                    ),
                }
            }
        );

    public static InlineKeyboardMarkup GroupIdBack { get; } =
        new(
            new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        "Как привязать группу?",
                        $"{CallbackData.Main}:{CallbackData.Bind}"
                    ),
                    InlineKeyboardButton.WithCallbackData(
                        "\u21a9 Назад",
                        $"{CallbackData.Main}:{CallbackData.Back}"
                    ),
                }
            }
        );

    public static InlineKeyboardMarkup BindBack { get; } =
        new(
            new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        "Как узнать ID группы?",
                        $"{CallbackData.GroupId}:{CallbackData.GroupId}"
                    ),
                    InlineKeyboardButton.WithCallbackData(
                        "\u21a9 Назад",
                        $"{CallbackData.Main}:{CallbackData.Back}"
                    )
                }
            }
        );

    public static InlineKeyboardMarkup AccountIdBack { get; } =
        new(
            new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        "\u21a9 Назад",
                        $"{CallbackData.Main}:{CallbackData.Back}"
                    )
                }
            }
        );

    public static InlineKeyboardMarkup CategoryBack { get; } =
        new(
            new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        "\u21a9 Назад",
                        $"{CallbackData.Main}:{CallbackData.Back}"
                    )
                }
            }
        );

    public static InlineKeyboardMarkup DigestBack { get; } =
        new(
            new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        "\u21a9 Назад",
                        $"{CallbackData.Main}:{CallbackData.Back}"
                    )
                }
            }
        );

    public static InlineKeyboardMarkup DeleteAccountBack { get; } =
        new(
            new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        "\u274c Удалить \u274c",
                        $"{CallbackData.Main}:{CallbackData.DeleteAccountConfirm}"
                    ),
                    InlineKeyboardButton.WithCallbackData(
                        "\u21a9 Назад",
                        $"{CallbackData.Main}:{CallbackData.Back}"
                    )
                }
            }
        );
}