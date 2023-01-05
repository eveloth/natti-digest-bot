using NattiDigestBot.Data.Models;

namespace NattiDigestBot.Services;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAll(CancellationToken cancellationToken);
    Task Create(Category category, CancellationToken cancellationToken);
    Task Update(Category category, CancellationToken cancellationToken);
    Task Delete(int id, CancellationToken cancellationToken);
}