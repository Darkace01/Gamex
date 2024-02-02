namespace Gamex.Service.Contract
{
    public interface ITournamentCategoryService
    {
        Task CreateCategory(TournamentCategoryCreateDTO category);
        Task DeleteCategory(Guid id);
        IQueryable<TournamentCategoryDTO> GetAllCategories();
        Task<TournamentCategoryDTO?> GetCategoryById(Guid id);
        Task<TournamentCategoryDTO> GetCategoryByName(string name);
        Task UpdateCategory(TournamentCategoryUpdateDTO category);
    }
}