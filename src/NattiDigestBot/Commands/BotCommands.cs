using Telegram.Bot.Types;

namespace NattiDigestBot.Commands;

public static class BotCommands
{
    public static List<BotCommand> PrivateChat { get; } =
        new()
        {
            new BotCommand { Command = "start", Description = "Запустить бота" },
            new BotCommand { Command = "help", Description = "Показать меню помощи" },
            new BotCommand { Command = "bind", Description = "Привязать группу" },
            new BotCommand { Command = "unbind", Description = "Отвязать группу" },
            new BotCommand { Command = "categories", Description = "Показать категории" },
            new BotCommand { Command = "new_category", Description = "Создать категорию" },
            new BotCommand
            {
                Command = "update_category",
                Description = "Отредактировать категорию"
            },
            new BotCommand { Command = "delete_category", Description = "Удалить категорию" },
            new BotCommand
            {
                Command = "digest",
                Description = "Перейти в режим составления дайджеста (требуется дата)"
            },
            new BotCommand
            {
                Command = "raw_preview",
                Description = "Показать ID сообщений в дайджесте"
            },
            new BotCommand { Command = "delete", Description = "Удалить сообщение из дайджеста" },
            new BotCommand { Command = "make", Description = "Сформировать дайджест" },
            new BotCommand
            {
                Command = "preview",
                Description = "Превью дайджеста (требуется дата)"
            },
            new BotCommand
            {
                Command = "edit",
                Description = "Отредактировать текст дайджеста или сообщение из дайджеста"
            },
            new BotCommand { Command = "send", Description = "Отправить дайджест (требуется дата" },
            new BotCommand { Command = "exit", Description = "Выйти из интерактивного режима" },
        };

    public static List<BotCommand> AllChats { get; } =
        new()
        {
            new BotCommand
            {
                Command = "confirm",
                Description = "Перейти в режим подтверждения группы / подтверидить группу в чате"
            }
        };
}