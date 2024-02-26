namespace Gamex.Test.UnitTest;

public class TagServiceTest: TestBase
{
    [Fact]
    public async Task GetTag_ShouldReturnTag()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetTag_ShouldReturnTag));
        var tagService = MockTagService(dbContext);
        var tagToGet = dbContext.Tags.FirstOrDefault();

        // Act
        var tag = await tagService.GetTagById(tagToGet.Id);

        // Assert
        Assert.NotNull(tag);
        Assert.Equal(tagToGet.Id, tag.Id);
    }

    [Fact]
    public async Task CreateTag_ShouldCreateTag()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(CreateTag_ShouldCreateTag));
        var tagService = MockTagService(dbContext);

        TagCreateDTO newTag = new()
        {
            Name = "Test Tag"
        };

        // Act
        var saved = await tagService.CreateTag(newTag);

        // Assert
        Assert.NotNull(saved);
        Assert.Equal(newTag.Name, saved.Name);
    }

    [Fact]
    public void GetAllTags_ShouldReturnAllTags()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetAllTags_ShouldReturnAllTags));
        var tagService = MockTagService(dbContext);

        // Act
        var tags = tagService.GetAllTags();

        // Assert
        Assert.NotNull(tags);
        Assert.Equal(3, tags.Count());
    }

    [Fact]
    public async Task UpdateTag_ShouldUpdateTag()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(UpdateTag_ShouldUpdateTag));
        var tagService = MockTagService(dbContext);
        var tagToUpdate = dbContext.Tags.FirstOrDefault();

        TagUpdateDTO updatedTag = new()
        {
            Id = tagToUpdate.Id,
            Name = "Updated Tag"
        };

        // Act
        var updated = await tagService.UpdateTag(updatedTag);

        // Assert
        Assert.True(updated);
    }

    [Fact]
    public async Task DeleteTag_ShouldDeleteTag()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(DeleteTag_ShouldDeleteTag));
        var tagService = MockTagService(dbContext);
        var tagToDelete = dbContext.Tags.FirstOrDefault();

        // Act
        var deleted = await tagService.DeleteTag(tagToDelete.Id);

        // Assert
        Assert.True(deleted);
    }

    [Fact]
    public async Task GetTagByName_ShouldReturnTag()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetTagByName_ShouldReturnTag));
        var tagService = MockTagService(dbContext);
        var tagToGet = dbContext.Tags.FirstOrDefault();

        // Act
        var tag = await tagService.GetTagByName(tagToGet.Name);

        // Assert
        Assert.NotNull(tag);
        Assert.Equal(tagToGet.Name, tag.Name);
    }

    #region Helpers
    private TagService MockTagService(GamexDbContext dbContext)
    {
        return new TagService(dbContext);
    }
    #endregion
}
