using NattiDigestBot.Data.Models;

namespace NattiDigestBot.Services;

public interface IAccountService
{
    Task<Account?> Get(long id, CancellationToken cancellationToken);
    Task Create(Account account, CancellationToken cancellationToken);
    Task BindGroup(Account account, CancellationToken cancellationToken);
    Task UnbindGroup(Account account, CancellationToken cancellationToken);
    Task ConfirmGroup(long id, long groupId, CancellationToken cancellationToken);
    Task Delete(long id, CancellationToken cancellationToken);
}