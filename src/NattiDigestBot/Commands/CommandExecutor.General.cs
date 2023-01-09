using NattiDigestBot.Commands.Interfaces;
using NattiDigestBot.Services.DbServices;
using NattiDigestBot.State;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace NattiDigestBot.Commands;

public partial class CommandExecutor : ICommandExecutor
{
    private readonly ILogger<CommandExecutor> _logger;
    private readonly ITelegramBotClient _botClient;
    private readonly IAccountService _accountService;

    public CommandExecutor(
        ITelegramBotClient botClient,
        IAccountService accountService,
        ILogger<CommandExecutor> logger
    )
    {
        _botClient = botClient;
        _accountService = accountService;
        _logger = logger;
    }

    public async Task SendUsage(Message message, CancellationToken cancellationToken)
    {
        var userId = message.Chat.Id;

        const string reply =
            "Что-то на эльфийском, не могу прочесть...\n\n"
            + "Чтобы узнать, как пользоваться ботом, напиши /help, "
            + "а чтобы получить информацию о любой команде — /info <i>команда</i>.";

        await _botClient.SendTextMessageAsync(
            userId,
            reply,
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken
        );
    }

    public async Task SendHelp(Message message, CancellationToken cancellationToken)
    {
        var userId = message.Chat.Id;

        const string reply =
            "Как ты уже знаешь, я — бот для создания дайджестов.\n\n"
            + "Для каждого, кто мне напишет, я создаю аккаунт, к которому можно привязать группу "
            + "и отправлять в неё дайджесты.\n\n"
            + "У меня есть нексколько режимов, обычный и пара интерактивных. В обычном режиме ты можешь "
            + "просто отправлять мне команды, а я буду на них отвечать. "
            + "Интерактивных режимов три — режим ожидания, в котором я жду, "
            + "когда ты подтвердишь свою группу, режим дайджеста  — в котором я попробую записать в дайджест "
            + "каждое сообщение, которое ты мне пришлёшь, и режим редактирования, в котором я жду, "
            + "пока ты пришлёшь мне отредактированный текст дайджеста, который я составил.\n\n"
            + "Вот список доступных команд. "
            + "Я советую тебе подробнее узнать о каждой, написав /info <i>команда</i>.";

        await _botClient.SendTextMessageAsync(
            userId,
            reply,
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken
        );

        const string commandList =
            "Команды, доступные только в режиме дайджеста, помечены эмодзи \u270f \n\n"
            + "/help - вывести эту инструкцию\n"
            + "/info <i>команда</i> - подробно о команде\n"
            + "/bind <i>id_группы</i> - привязать группу\n"
            + "/unbind - отвязать группу\n"
            + "/confirm - перейти в режим подтверждения группы\n"
            + "/delete_account - удалить аккаунт\n"
            + "/add_category <i>категория - описание</i> - добавить категорию\n"
            + "/categories - управление категориями\n"
            + "/digest <i>01/01/19</i> - перейти в режим составления дайджеста\n"
            + "/raw_preview - превью дайджеста без форматирования \u270f \n"
            + "/make - составить дайджест из присланных сообщений \u270f \n"
            + "/preview <i>01/01/19</i> - превью дайджеста с форматированием\n"
            + "/edit <i>01/01/19</i> - перейти в режим редактирования текста дайджеста\n"
            + "/send <i>01/01/19</i> - отправить дайджест";

        await _botClient.SendTextMessageAsync(
            userId,
            commandList,
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken
        );
    }

    public Task SendCommandInfo(Message message, CancellationToken cancellationToken)
    {

        var userId = message.Chat.Id;
        throw new NotImplementedException();
    }

    public async Task ExitAndSetNormalMode(Message message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Executing command {Command}", nameof(ExitAndSetNormalMode));

        var userId = message.Chat.Id;

        if (StateStorage.IsConfirmationRequestedForUser(userId))
        {
            var account = await _accountService.Get(userId, cancellationToken);

            StateStorage.RemoveFromWaitingForConfirmationList(account!.GroupId!.Value);
        }

        StateStorage.SetChatModeFor(userId, ChatMode.Normal);

        const string reply = "Теперь ты находишься в обычном режиме и можешь давать боту команды.";
        await _botClient.SendTextMessageAsync(userId, reply, cancellationToken: cancellationToken);
    }
}