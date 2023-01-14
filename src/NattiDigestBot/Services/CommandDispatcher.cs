using NattiDigestBot.Commands.Interfaces;
using NattiDigestBot.Replies.Menus;
using NattiDigestBot.State;
using Telegram.Bot.Types;

namespace NattiDigestBot.Services;

public class CommandDispatcher : ICommandDispatcher
{
    private readonly ICommandExecutor _commandExecutor;
    private readonly ICallbackQueryProcessor _callbackQueryProcessor;

    public CommandDispatcher(
        ICommandExecutor commandExecutor,
        ICallbackQueryProcessor callbackQueryProcessor
    )
    {
        _commandExecutor = commandExecutor;
        _callbackQueryProcessor = callbackQueryProcessor;
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
            "/help" => _commandExecutor.SendHelp(message, cancellationToken),
            "/bind" => _commandExecutor.Bind(message, cancellationToken),
            "/unbind" => _commandExecutor.Unbind(message, cancellationToken),
            "/confirm" => _commandExecutor.StartConfirmationProcess(message, cancellationToken),
            "/categories" => _commandExecutor.ShowCategories(message, cancellationToken),
            "/new_category" => _commandExecutor.NewCategory(message, cancellationToken),
            "/update_category" => _commandExecutor.UpdateCategory(message, cancellationToken),
            "/delete_category" => _commandExecutor.DeleteCategory(message, cancellationToken),
            "/digest" => _commandExecutor.Digest(message, cancellationToken),
            "/preview" => _commandExecutor.Preview(message, cancellationToken),
            "/edit" => _commandExecutor.Edit(message, cancellationToken),
            "/send" => _commandExecutor.Send(message, cancellationToken),
            _ => _commandExecutor.SendUsage(message, cancellationToken)
        };

        await action;
    }

    public async Task HandleDigestMode(Message message, CancellationToken cancellationToken)
    {
        var command = message.Text!.Split(' ').ElementAtOrDefault(0);

        if (command is null)
        {
            return;
        }

        var action = command switch
        {
            "/raw_preview" => _commandExecutor.RawPreview(message, cancellationToken),
            "/categories" => _commandExecutor.ShowCategories(message, cancellationToken),
            "/delete" => _commandExecutor.RemoveEntry(message, cancellationToken),
            "/make" => _commandExecutor.Make(message, cancellationToken),
            "/exit" => _commandExecutor.ExitAndSetNormalMode(message, cancellationToken),
            _ => _commandExecutor.AddEntry(message, cancellationToken)
        };

        await action;
    }

    public async Task HandleEditMode(Message message, CancellationToken cancellationToken)
    {
        var command = message.Text!.Split(' ').ElementAtOrDefault(0);

        if (command is null)
        {
            return;
        }

        var action = command switch
        {
            "/exit" => _commandExecutor.ExitAndSetNormalMode(message, cancellationToken),
            _ => _commandExecutor.UpdateDigestText(message, cancellationToken)
        };

        await action;
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

    public async Task HandleConfirmationFromGroup(
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

    public async Task HandleMainMenuCallbackQuery(
        CallbackQuery query,
        CancellationToken cancellationToken
    )
    {
        var menuSection = query.Data!.Split(':')[1];

        var action = menuSection switch
        {
            _ when menuSection.Equals(CallbackData.GroupId)
                => _callbackQueryProcessor.ShowGroupIdInfo(query, cancellationToken),
            _ when menuSection.Equals(CallbackData.Bind)
                => _callbackQueryProcessor.ShowGroupBindingInfo(query, cancellationToken),
            _ when menuSection.Equals(CallbackData.Categories)
                => _callbackQueryProcessor.ShowCategoriesInfo(query, cancellationToken),
            _ when menuSection.Equals(CallbackData.Digest)
                => _callbackQueryProcessor.ShowDigestInfo(query, cancellationToken),
            _ when menuSection.Equals(CallbackData.AccountId)
                => _callbackQueryProcessor.ShowMyAccountId(query, cancellationToken),
            _ when menuSection.Equals(CallbackData.DeleteAccountPropmt)
                => _callbackQueryProcessor.ShowDeleteAccountPrompt(query, cancellationToken),
            _ when menuSection.Equals(CallbackData.DeleteAccountConfirm)
                => _callbackQueryProcessor.DeleteAccount(query, cancellationToken),
            _ when menuSection.Equals(CallbackData.PrivateGroups)
                => _callbackQueryProcessor.ShowPrivateGroupsInfo(query, cancellationToken),
            _ when menuSection.Equals(CallbackData.Back)
                => _callbackQueryProcessor.BackToMain(query, cancellationToken),
            _ => Task.CompletedTask
        };

        await action;
    }

    public async Task HandleHtmlReferenceMenuCallbackQuery(
        CallbackQuery query,
        CancellationToken cancellationToken
    )
    {
        var menuSection = query.Data!.Split(':')[1];

        var action = menuSection switch
        {
            _ when menuSection.Equals(CallbackData.Reference)
                => _callbackQueryProcessor.ShowHtmlReference(query, cancellationToken),
            _ when menuSection.Equals(CallbackData.Back)
                => _callbackQueryProcessor.BackToEdit(query, cancellationToken),
            _ => Task.CompletedTask
        };

        await action;
    }
}