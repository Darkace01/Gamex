﻿namespace Gamex.Service.Implementation;

public class TournamentCategoryService
{
    private readonly GamexDbContext _context;

    public TournamentCategoryService(GamexDbContext context)
    {
        _context = context;
    }

    public IQueryable<CategoryDTO> GetAllCategories()
    {
        return _context.TournamentCategories
            .AsNoTracking()
            .Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name
            });
    }

    public async Task<CategoryDTO?> GetCategoryById(Guid id)
    {
        return await GetAllCategories().FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task CreateCategory(CategoryDTO category)
    {
        var newCategory = new TournamentCategory
        {
            Id = Guid.NewGuid(),
            Name = category.Name
        };

        _context.TournamentCategories.Add(newCategory);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCategory(CategoryDTO category)
    {
        var existingCategory = await _context.TournamentCategories.FirstOrDefaultAsync(c => c.Id == category.Id);
        if (existingCategory == null)
            throw new Exception("Category not found");

        existingCategory.Name = category.Name;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCategory(Guid id)
    {
        var existingCategory = await _context.TournamentCategories.FirstOrDefaultAsync(c => c.Id == id);
        if (existingCategory == null)
            throw new Exception("Category not found");

        _context.TournamentCategories.Remove(existingCategory);
        await _context.SaveChangesAsync();
    }
}
