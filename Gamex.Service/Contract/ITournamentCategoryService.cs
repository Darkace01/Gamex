namespace Gamex.Service.Contract
{
    /// <summary>
    /// Represents the interface for managing tournament categories.
    /// </summary>
    public interface ITournamentCategoryService
    {
        /// <summary>
        /// Creates a new tournament category.
        /// </summary>
        /// <param name="category">The tournament category to create.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateCategory(TournamentCategoryCreateDTO category);

        /// <summary>
        /// Deletes a tournament category by its ID.
        /// </summary>
        /// <param name="id">The ID of the tournament category to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteCategory(Guid id);

        /// <summary>
        /// Gets all tournament categories.
        /// </summary>
        /// <returns>An <see cref="IQueryable{TournamentCategoryDTO}"/> representing the collection of tournament categories.</returns>
        IQueryable<TournamentCategoryDTO> GetAllCategories();

        /// <summary>
        /// Gets a tournament category by its ID.
        /// </summary>
        /// <param name="id">The ID of the tournament category to retrieve.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the tournament category, or null if not found.</returns>
        Task<TournamentCategoryDTO?> GetCategoryById(Guid id);

        /// <summary>
        /// Gets a tournament category by its name.
        /// </summary>
        /// <param name="name">The name of the tournament category to retrieve.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the tournament category.</returns>
        Task<TournamentCategoryDTO> GetCategoryByName(string name);

        /// <summary>
        /// Updates a tournament category.
        /// </summary>
        /// <param name="category">The tournament category to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateCategory(TournamentCategoryUpdateDTO category);
    }
}