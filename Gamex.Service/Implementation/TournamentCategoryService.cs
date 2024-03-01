namespace Gamex.Service.Implementation;

public class TournamentCategoryService(GamexDbContext context) : ITournamentCategoryService
{
    private readonly GamexDbContext _context = context;

    /// <summary>
    /// Retrieves all tournament categories.
    /// </summary>
    /// <returns>An <see cref="IQueryable{TournamentCategoryDTO}"/> representing the collection of tournament categories.</returns>
    public IQueryable<TournamentCategoryDTO> GetAllCategories()
    {
        return _context.TournamentCategories
            .AsNoTracking()
            .Select(c => new TournamentCategoryDTO
            {
                Id = c.Id,
                Name = c.Name
            });
    }

    /// <summary>
    /// Retrieves a tournament category by its ID.
    /// </summary>
    /// <param name="id">The ID of the tournament category.</param>
    /// <returns>A <see cref="Task{TournamentCategoryDTO}"/> representing the asynchronous operation. The task result contains the tournament category with the specified ID, or null if not found.</returns>
    public async Task<TournamentCategoryDTO?> GetCategoryById(Guid id)
    {
        return await GetAllCategories().FirstOrDefaultAsync(c => c.Id == id);
    }

    /// <summary>
    /// Creates a new tournament category.
    /// </summary>
    /// <param name="category">The tournament category to create.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task CreateCategory(TournamentCategoryCreateDTO category)
    {
        var newCategory = new TournamentCategory
        {
            Name = category.Name
        };

        _context.TournamentCategories.Add(newCategory);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing tournament category.
    /// </summary>
    /// <param name="category">The tournament category to update.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="Exception">Thrown when the category with the specified ID is not found.</exception>
    public async Task UpdateCategory(TournamentCategoryUpdateDTO category)
    {
        var existingCategory = await _context.TournamentCategories.FirstOrDefaultAsync(c => c.Id == category.Id);
        if (existingCategory == null)
            throw new Exception("Category not found");

        existingCategory.Name = category.Name;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a tournament category by its ID.
    /// </summary>
    /// <param name="id">The ID of the tournament category to delete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="Exception">Thrown when the category with the specified ID is not found.</exception>
    public async Task DeleteCategory(Guid id)
    {
        var existingCategory = await _context.TournamentCategories.FirstOrDefaultAsync(c => c.Id == id);
        if (existingCategory == null)
            throw new Exception("Category not found");

        _context.TournamentCategories.Remove(existingCategory);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Retrieves a tournament category by its name.
    /// </summary>
    /// <param name="name">The name of the tournament category.</param>
    /// <returns>A <see cref="Task{TournamentCategoryDTO}"/> representing the asynchronous operation. The task result contains the tournament category with the specified name.</returns>
    public async Task<TournamentCategoryDTO> GetCategoryByName(string name)
    {
        return await GetAllCategories().FirstOrDefaultAsync(c => c.Name.ToLower().Trim() == name.ToLower());
    }

}
