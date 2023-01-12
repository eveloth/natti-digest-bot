using Microsoft.EntityFrameworkCore;
using NattiDigestBot.Data;
using NattiDigestBot.Domain;

namespace NattiDigestBot.Services.DbServices;

public class DigestService : IDigestService
{
    private readonly DigestContext _context;
    private readonly ILogger<DigestService> _logger;

    public DigestService(DigestContext context, ILogger<DigestService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Digest?> Get(
        long accountId,
        DateOnly date,
        CancellationToken cancellationToken
    )
    {
        return await _context.Digests
            .AsNoTracking()
            .SingleOrDefaultAsync(
                d => d.AccountId == accountId && d.Date == date,
                cancellationToken
            );
    }

    public async Task Create(Digest digest, CancellationToken cancellationToken)
    {
        await _context.Digests.AddAsync(digest, cancellationToken);

        _logger.LogInformation(
            "Creating digest as of {Date} for account ID {AccountId}",
            digest.Date,
            digest.AccountId
        );

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task Update(Digest digest, CancellationToken cancellationToken)
    {
        var existingDigest = await _context.Digests.SingleOrDefaultAsync(
            d => d.AccountId == digest.AccountId && d.Date == digest.Date,
            cancellationToken
        );

        if (existingDigest is null)
        {
            return;
        }

        existingDigest.DigestText = digest.DigestText;
        existingDigest.IsSent = digest.IsSent;

        _logger.LogInformation(
            "Updating digest as of {Date} for account ID {AccountId}",
            digest.Date,
            digest.AccountId
        );

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddEntry(DigestEntry entry, CancellationToken cancellationToken)
    {
        var digest = await _context.Digests.SingleOrDefaultAsync(
            d => d.AccountId == entry.DigestId && d.Date == entry.Date,
            cancellationToken
        );

        digest!.DigestEntries.Add(entry);

        _logger.LogInformation(
            "Adding an entry ID {EntryId} to the digest as of {Date} for account ID {AccountId}",
            entry.DigestEntryId,
            digest.Date,
            digest.AccountId
        );

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteEntry(DigestEntry entry, CancellationToken cancellationToken)
    {
        var digest = await _context.Digests.SingleOrDefaultAsync(
            d => d.AccountId == entry.DigestId && d.Date == entry.Date,
            cancellationToken
        );

        var entryToDelete = digest!.DigestEntries.Find(e => e.DigestEntryId == entry.DigestEntryId);

        if (entryToDelete is null)
        {
            return false;
        }

        digest.DigestEntries.Remove(entryToDelete);

        _logger.LogInformation(
            "Removing an entry ID {EntryId} from the digest as of {Date} for account ID {AccountId}",
            entry.DigestEntryId,
            digest.Date,
            digest.AccountId
        );

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}