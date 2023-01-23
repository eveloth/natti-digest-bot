using Telegram.Bot.Types;

namespace NattiDigestBot.Commands.Interfaces;

public partial interface ICommandExecutor
{
    Task AddEntry(Message message, CancellationToken cancellationToken);
    Task UpdateEntry(Message message, CancellationToken cancellationToken);
    Task RemoveEntry(Message message, CancellationToken cancellationToken);
    Task RawPreview(Message message, CancellationToken cancellationToken);
    Task Make(Message message, CancellationToken cancellationToken);

}