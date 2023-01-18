using Microsoft.EntityFrameworkCore;
using NattiDigestBot.Data;
using NattiDigestBot.Domain;

namespace NattiDigestBot.Services.DbServices;

public class AccountService : IAccountService
{
    private readonly DigestContext _context;
    private readonly ILogger<AccountService> _logger;

    public AccountService(DigestContext context, ILogger<AccountService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Account?> Get(long id, CancellationToken cancellationToken)
    {
        return await _context.Accounts
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.AccountId == id, cancellationToken);
    }

    public async Task Create(Account account, CancellationToken cancellationToken)
    {
        await _context.Accounts.AddAsync(account, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Creating an account ID {AccountId}", account.AccountId);
    }

    public async Task BindGroup(long accountId, long groupId, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts.SingleAsync(
            x => x.AccountId == accountId,
            cancellationToken
        );
        account.GroupId = groupId;

        _logger.LogInformation(
            "Binding group ID {GroupId} to account ID {AccountId}",
            groupId,
            accountId
        );

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UnbindGroup(long id, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts.SingleAsync(
            x => x.AccountId == id,
            cancellationToken
        );

        _logger.LogInformation(
            "Unbinding group ID {GroupId} from account ID {AccountId}",
            account.GroupId,
            account.AccountId
        );

        account.GroupId = null;
        account.IsGroupConfirmed = false;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task ConfirmGroupForAccount(long id, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts.SingleAsync(
            x => x.AccountId == id,
            cancellationToken
        );
        account.IsGroupConfirmed = true;

        _logger.LogInformation(
            "Confirming group ID {GroupId} for account ID {AccountId}",
            account.GroupId,
            account.AccountId
        );

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(long id, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts.SingleOrDefaultAsync(
            x => x.AccountId == id,
            cancellationToken
        );

        if (account is null)
        {
            return;
        }

        _logger.LogInformation("Deleting an account ID {AccountId}", account.AccountId);

        _context.Accounts.Remove(account);
        await _context.SaveChangesAsync(cancellationToken);
    }
}