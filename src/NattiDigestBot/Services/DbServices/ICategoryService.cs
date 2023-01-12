using NattiDigestBot.Domain;

namespace NattiDigestBot.Services.DbServices;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAll(long accountId, CancellationToken cancellationToken);
    Task<bool> Create(Category category, CancellationToken cancellationToken);
    Task<bool> Update(Category category, CancellationToken cancellationToken);
    Task<bool> Delete(int id, long aacountId, CancellationToken cancellationToken);
}