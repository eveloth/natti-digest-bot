using Microsoft.EntityFrameworkCore;
using NattiDigestBot.Data;
using NattiDigestBot.Domain;

namespace NattiDigestBot.Services.DbServices;

public class CategoryService : ICategoryService
{
    private readonly DigestContext _context;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(DigestContext context, ILogger<CategoryService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Category>> GetAll(
        long accountId,
        CancellationToken cancellationToken
    )
    {
        return await _context.Categories
            .AsNoTracking()
            .Where(c => c.AccountId == accountId)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> Create(Category category, CancellationToken cancellationToken)
    {
        var existingCategory = await _context.Categories.SingleOrDefaultAsync(
            c => c.AccountId == category.AccountId && c.Keyword == category.Keyword,
            cancellationToken
        );

        if (existingCategory is not null)
        {
            _logger.LogInformation(
                "Rejected: category keyword was taken ({CategoryName}) for account ID {AccountId}",
                existingCategory.Keyword,
                existingCategory.AccountId
            );
            return false;
        }

        await _context.Categories.AddAsync(category, cancellationToken);

        _logger.LogInformation(
            "Creating a category ID {CategoryId} for account ID {AccountId}",
            category.CategoryId,
            category.AccountId
        );

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> Update(Category category, CancellationToken cancellationToken)
    {
        var existingCategory = await _context.Categories.SingleOrDefaultAsync(
            c => c.AccountId == category.AccountId && c.CategoryId == category.CategoryId,
            cancellationToken
        );

        if (existingCategory is null)
        {
            _logger.LogInformation(
                "A category ID {CategoryId} for account ID {AccountId} was not found",
                category.CategoryId,
                category.AccountId
            );

            return false;
        }

        existingCategory.Keyword = category.Keyword;
        existingCategory.Description = category.Description;

        _logger.LogInformation(
            "Updating a category ID {CategoryId} for account ID {AccountId}",
            category.CategoryId,
            category.AccountId
        );

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> Delete(int id, long accountId, CancellationToken cancellationToken)
    {
        var existingCategory = await _context.Categories.SingleOrDefaultAsync(
            c => c.AccountId == accountId && c.CategoryId == id,
            cancellationToken
        );

        if (existingCategory is null)
        {
            _logger.LogInformation(
                "A category ID {CategoryId} for account ID {AccountId} was not found",
                id,
                accountId
            );

            return false;
        }

        _logger.LogInformation(
            "Deleting a category ID {CategoryId} for account ID {AccountId}",
            existingCategory.CategoryId,
            existingCategory.AccountId
        );

        _context.Categories.Remove(existingCategory);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}