using NattiDigestBot.Commands.Interfaces;
using NattiDigestBot.State;
using Telegram.Bot.Types;

namespace NattiDigestBot.Services;

public class CommandDispatcher : ICommandDispatcher
{
    private readonly ICommandExecutor _commandExecutor;

    public CommandDispatcher(ICommandExecutor commandExecutor)
    {
        _commandExecutor = commandExecutor;
    }

    public async Task HandleNormalMode(Message message, CancellationToken cancellationToken)
    {
        var command = message.Text!.Split(' ').ElementAtOrDefault(0);

        if (command is null)
        {
            return;
        }

        var action = command switch
        {
            "/start" => _commandExecutor.Start(message, cancellationToken),
            "/bind" => _commandExecutor.Bind(message, cancellationToken),
            "/unbind" => _commandExecutor.Unbind(message, cancellationToken),
            "/confirm" => _commandExecutor.Confirm(message, cancellationToken),
            "/digest" => _commandExecutor.Digest(message, cancellationToken),
            "/exit" => _commandExecutor.ExitAndSetNormalMode(message, cancellationToken),
            _ => _commandExecutor.SendUsage(message, cancellationToken)
        };

        await action;
    }

    public Task HandleDigestMode(Message message, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task HandleEditMode(Message message, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task HandleWaitingForConfirmationMode(
        Message message,
        CancellationToken cancellationToken
    )
    {
        var command = message.Text!.Split(' ').ElementAtOrDefault(0);

        if (command is null)
        {
            return;
        }

        var action = command switch
        {
            "/exit" => _commandExecutor.ExitAndSetNormalMode(message, cancellationToken),
            _ => _commandExecutor.HandleMessageInConfirmationMode(message, cancellationToken)
        };

        await action;
    }

    public async Task ReceiveConfirmationFromGroup(
        Message message,
        CancellationToken cancellationToken
    )
    {
        var command = message.Text!.Split(' ').ElementAtOrDefault(0);

        if (command is null)
        {
            return;
        }

        var action = command switch
        {
            _ when command == $"/confirm@{StateStorage.BotName}"
                => _commandExecutor.ConfirmGroup(message, cancellationToken),
            _ => Task.CompletedTask
        };

        await action;
    }
}