using NattiDigestBot.Data.Models;
using NattiDigestBot.Domain;

namespace NattiDigestBot.Services;

public interface IDigestService
{
    Task<Digest> Get(long accountId, DateOnly date, CancellationToken cancellationToken);
    Task Create(Digest digest, CancellationToken cancellationToken);
    Task AddEntry(DigestEntry entry, CancellationToken cts);
    Task DeleteEntry(long digestId, DateOnly date, CancellationToken cancellationToken);
}