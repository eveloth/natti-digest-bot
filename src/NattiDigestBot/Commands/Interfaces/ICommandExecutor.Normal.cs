using Telegram.Bot.Types;

namespace NattiDigestBot.Commands.Interfaces;

public partial interface ICommandExecutor
{
    Task Start(Message message, CancellationToken cancellationToken);
    Task Bind(Message message, CancellationToken cancellationToken);
    Task Unbind(Message message, CancellationToken cancellationToken);
    Task StartConfirmationProcess(Message message, CancellationToken cancellationToken);
    Task NewCategory(Message message, CancellationToken cancellationToken);
    Task ShowCategories(Message message, CancellationToken cancellationToken);
    Task EditCategory(Message message, CancellationToken cancellationToken);
    Task DeleteCategory(Message message, CancellationToken cancellationToken);
    Task Digest(Message message, CancellationToken cancellationToken);
    Task Preview(Message message, CancellationToken cancellationToken);
    Task Edit(Message message, CancellationToken cancellationToken);
    Task Send(Message message, CancellationToken cancellationToken);
}