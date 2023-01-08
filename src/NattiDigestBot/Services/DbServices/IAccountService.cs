using NattiDigestBot.Domain;

namespace NattiDigestBot.Services.DbServices;

public interface IAccountService
{
    Task<Account?> Get(long id, CancellationToken cancellationToken);
    Task Create(Account account, CancellationToken cancellationToken);
    Task BindGroup(long accountId, long groupId, CancellationToken cancellationToken);
    Task UnbindGroup(long id, CancellationToken cancellationToken);
    Task ConfirmGroupForAccount(long id, CancellationToken cancellationToken);
    Task Delete(long id, CancellationToken cancellationToken);
}