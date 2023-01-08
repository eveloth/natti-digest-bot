using NattiDigestBot.Domain;

namespace NattiDigestBot.Services.DbServices;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAll(CancellationToken cancellationToken);
    Task Create(Category category, CancellationToken cancellationToken);
    Task Update(Category category, CancellationToken cancellationToken);
    Task Delete(int id, CancellationToken cancellationToken);
}