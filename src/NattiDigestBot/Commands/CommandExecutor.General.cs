using NattiDigestBot.Commands.Interfaces;
using NattiDigestBot.Extensions;
using NattiDigestBot.Replies;
using NattiDigestBot.Replies.Menus;
using NattiDigestBot.Services.DbServices;
using NattiDigestBot.State;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace NattiDigestBot.Commands;

public partial class CommandExecutor : ICommandExecutor
{
    private readonly ILogger<CommandExecutor> _logger;
    private readonly ITelegramBotClient _botClient;
    private readonly IAccountService _accountService;
    private readonly ICategoryService _categoryService;
    private readonly IDigestService _digestService;

    public CommandExecutor(
        ITelegramBotClient botClient,
        IAccountService accountService,
        ILogger<CommandExecutor> logger,
        ICategoryService categoryService,
        IDigestService digestService
    )
    {
        _botClient = botClient;
        _accountService = accountService;
        _logger = logger;
        _categoryService = categoryService;
        _digestService = digestService;
    }

    public async Task SendUsage(Message message, CancellationToken cancellationToken)
    {
        var userId = message.Chat.Id;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(SendUsage),
            userId
        );

        await _botClient.SendReply(userId, GeneralReplies.UsageReply, cancellationToken);
    }

    public async Task SendHelp(Message message, CancellationToken cancellationToken)
    {
        var userId = message.Chat.Id;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(SendHelp),
            userId
        );

        await _botClient.SendReply(userId, HelpMenu.Entrypoint, cancellationToken);
    }

    public async Task SendCommandInfo(Message message, CancellationToken cancellationToken)
    {
        var userId = message.Chat.Id;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(SendCommandInfo),
            userId
        );
    }

    public async Task ExitAndSetNormalMode(Message message, CancellationToken cancellationToken)
    {
        var userId = message.Chat.Id;

        _logger.LogInformation(
            "Executing command {Command} for account ID {AccountId}",
            nameof(ExitAndSetNormalMode),
            userId
        );

        if (StateStorage.IsConfirmationRequestedForUser(userId))
        {
            var account = await _accountService.Get(userId, cancellationToken);

            StateStorage.RemoveFromWaitingForConfirmationList(account!.GroupId!.Value);
        }

        StateStorage.SetChatModeFor(userId, ChatMode.Normal);

        await _botClient.SendReply(userId, GeneralReplies.ReturningToNormalMode, cancellationToken);
    }
}