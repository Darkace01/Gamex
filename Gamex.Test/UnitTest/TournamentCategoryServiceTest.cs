namespace Gamex.Test.UnitTest;

public class TournamentCategoryServiceTest: TestBase
{
    [Fact]
    public async Task GetTournamentCategory_ShouldReturnTournamentCategory()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetTournamentCategory_ShouldReturnTournamentCategory));
        var tournamentCategoryService = MockTournamentCategoryService(dbContext);
        var tournamentCategoryToGet = dbContext.TournamentCategories.FirstOrDefault();

        // Act
        var tournamentCategory = await tournamentCategoryService.GetCategoryById(tournamentCategoryToGet.Id);

        // Assert
        Assert.NotNull(tournamentCategory);
        Assert.Equal(tournamentCategoryToGet.Id, tournamentCategory.Id);
    }

    [Fact]
    public async Task CreateTournamentCategory_ShouldCreateTournamentCategory()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(CreateTournamentCategory_ShouldCreateTournamentCategory));
        var tournamentCategoryService = MockTournamentCategoryService(dbContext);

        // Act
        var newCategory = new CategoryDTO
        {
            Name = "Test Category NEW"
        };
        await tournamentCategoryService.CreateCategory(newCategory);

        // Assert
        Assert.Equal(4, dbContext.TournamentCategories.Count());
        Assert.Equal(newCategory.Name, dbContext.TournamentCategories.FirstOrDefault(x => x.Name == newCategory.Name).Name);
    }

    [Fact]
    public async Task UpdateTournamentCategory_ShouldUpdateTournamentCategory()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(UpdateTournamentCategory_ShouldUpdateTournamentCategory));
        var tournamentCategoryService = MockTournamentCategoryService(dbContext);
        var categoryToUpdate = dbContext.TournamentCategories.FirstOrDefault();

        // Act
        var updatedCategory = new CategoryDTO
        {
            Id = categoryToUpdate.Id,
            Name = "Updated Category"
        };
        await tournamentCategoryService.UpdateCategory(updatedCategory);

        // Assert
        Assert.Equal(3, dbContext.TournamentCategories.Count());
        Assert.Equal(updatedCategory.Name, dbContext.TournamentCategories.FirstOrDefault(x => x.Id == categoryToUpdate.Id).Name);
    }

    [Fact]
    public async Task DeleteTournamentCategory_ShouldDeleteTournamentCategory()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(DeleteTournamentCategory_ShouldDeleteTournamentCategory));
        var tournamentCategoryService = MockTournamentCategoryService(dbContext);
        var categoryToDelete = dbContext.TournamentCategories.FirstOrDefault();

        // Act
        await tournamentCategoryService.DeleteCategory(categoryToDelete.Id);

        // Assert
        Assert.Equal(2, dbContext.TournamentCategories.Count());
    }

    [Fact]
    public void GetAllTournamentCategories_ShouldReturnAllTournamentCategories()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetAllTournamentCategories_ShouldReturnAllTournamentCategories));
        var tournamentCategoryService = MockTournamentCategoryService(dbContext);

        // Act
        var categories = tournamentCategoryService.GetAllCategories();

        // Assert
        Assert.NotNull(categories);
        Assert.Equal(3, categories.Count());
    }

    [Fact]
    public void GetAllTournamentCategories_ShouldReturnEmpty()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetAllTournamentCategories_ShouldReturnEmpty));
        var tournamentCategoryService = MockTournamentCategoryService(dbContext);
        dbContext.TournamentCategories.RemoveRange(dbContext.TournamentCategories);
        dbContext.SaveChanges();

        // Act
        var categories = tournamentCategoryService.GetAllCategories();

        // Assert
        Assert.NotNull(categories);
        Assert.Empty(categories);
    }

    [Fact]
    public async Task GetTournamentCategory_ShouldReturnNull()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetTournamentCategory_ShouldReturnNull));
        var tournamentCategoryService = MockTournamentCategoryService(dbContext);
        var tournamentCategoryToGet = dbContext.TournamentCategories.FirstOrDefault();
        dbContext.TournamentCategories.RemoveRange(dbContext.TournamentCategories);
        dbContext.SaveChanges();

        // Act
        var tournamentCategory = await tournamentCategoryService.GetCategoryById(tournamentCategoryToGet.Id);

        // Assert
        Assert.Null(tournamentCategory);
    }
    #region Helpers
    private TournamentCategoryService MockTournamentCategoryService(GamexDbContext dbContext)
    {
        return new TournamentCategoryService(dbContext);
    }
    #endregion
}
