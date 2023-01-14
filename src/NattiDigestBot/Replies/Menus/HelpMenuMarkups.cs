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
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        "\u2757 О приватных группах",
                        $"{CallbackData.Main}:{CallbackData.PrivateGroups}"
                    )
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
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        "\u2757 О приватных группах",
                        $"{CallbackData.Main}:{CallbackData.PrivateGroups}"
                    )
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
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        "\u2757 О приватных группах",
                        $"{CallbackData.Main}:{CallbackData.PrivateGroups}"
                    )
                }
            }
        );

    public static InlineKeyboardMarkup Back { get; } =
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