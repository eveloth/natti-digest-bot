using Telegram.Bot.Types;

namespace NattiDigestBot.Commands.Interfaces;

public partial interface ICommandExecutor
{
    Task Start(Message message, CancellationToken cancellationToken);
    Task Bind(Message message, CancellationToken cancellationToken);
    Task Unbind(Message message, CancellationToken cancellationToken);
    Task StartConfirmationProcess(Message message, CancellationToken cancellationToken);
    Task DeleteAccount(Message message, CancellationToken cancellationToken);
    Task Digest(Message message, CancellationToken cancellationToken);
}