using System.Diagnostics;
using NattiDigestBot.Commands.Interfaces;
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
        throw new NotImplementedException();
    }

    public async Task SendHelp(Message message, CancellationToken cancellationToken)
    {
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