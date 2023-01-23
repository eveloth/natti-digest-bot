using NattiDigestBot.Domain;

namespace NattiDigestBot.Services.DbServices;

public interface IDigestService
{
    Task<Digest?> Get(long accountId, DateOnly date, CancellationToken cancellationToken);
    Task Create(Digest digest, CancellationToken cancellationToken);
    Task Update(Digest digest, CancellationToken cancellationToken);
    Task<DigestEntry?> GetEntry(long accountId, DateOnly date, int entryId, CancellationToken cancellationToken);
    Task AddEntry(DigestEntry entry, CancellationToken cancellationToken);
    Task<bool> UpdateEntry(DigestEntry entry, CancellationToken cancellationToken);
    Task<bool> DeleteEntry(DigestEntry entry, CancellationToken cancellationToken);
}