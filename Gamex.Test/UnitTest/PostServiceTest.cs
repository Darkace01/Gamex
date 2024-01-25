namespace Gamex.Test.UnitTest;

public class PostServiceTest: TestBase
{
    [Fact]
    public async Task GetPost_ShouldReturnPost()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetPost_ShouldReturnPost));
        var postService = MockPostService(dbContext);
        var postToGet = dbContext.Posts.FirstOrDefault();

        // Act
        var post = await postService.GetPost(postToGet.Id);

        // Assert
        Assert.NotNull(post);
        Assert.Equal(postToGet.Id, post.Id);
    }

    [Fact]
    public async Task CreatePost_ShouldCreatePost()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(CreatePost_ShouldCreatePost));
        var postService = MockPostService(dbContext);

        var testUser = dbContext.Users.FirstOrDefault();

        //PostCreateDTO newPost = new("Test Post", "Test Post Content",false,null,null,testUser.Id);
        PostCreateDTO newPost = new()
        {
            Title = "Test Post",
            Content = "Test Post Content",
            IsArchived = false,
            PictureId = null,
            Picture = null,
            UserId = testUser.Id
        };

        // Act
        var saved = await postService.CreatePost(newPost,testUser);

        // Assert
        Assert.True(saved);
        // Add more assertions to check the properties of the created saved
    }

    [Fact]
    public async Task UpdatePost_ShouldUpdatePost()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(UpdatePost_ShouldUpdatePost));
        var postService = MockPostService(dbContext);
        var postToUpdate = dbContext.Posts.FirstOrDefault();
        var testUser = dbContext.Users.FirstOrDefault();

        PostUpdateDTO updatedPost = new()
        {
            Id = postToUpdate.Id,
            Title = "Test Post",
            Content = "Test Post Content",
            IsArchived = false,
            PictureId = null,
            Picture = null,
            UserId = testUser.Id
        };
        //PostUpdateDTO updatedPost = new(postToUpdate.Id,"Test Post", "Test Post Content",false,null,null,testUser.Id);

        // Act
        var updated = await postService.UpdatePost(updatedPost,testUser);

        // Assert
        var updatedPostInDb = dbContext.Posts.FirstOrDefault(p => p.Id == postToUpdate.Id);
        Assert.NotNull(updatedPostInDb);
        Assert.True(updated);
        Assert.Equal(updatedPost.Title, updatedPostInDb.Title);
        // Add more assertions to check the properties of the updated saved
    }

    [Fact]
    public async Task DeletePost_ShouldDeletePost()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(DeletePost_ShouldDeletePost));
        var postService = MockPostService(dbContext);
        var postToDelete = dbContext.Posts.FirstOrDefault();
        var testUser = dbContext.Users.FirstOrDefault();

        // Act
        var deleted = await postService.DeletePost(postToDelete.Id,testUser);

        // Assert
        var deletedPostInDb = dbContext.Posts.FirstOrDefault(p => p.Id == postToDelete.Id);
        Assert.Null(deletedPostInDb);
        Assert.True(deleted);
    }

    #region Helpers
    private PostService MockPostService(GamexDbContext dbContext)
    {
        return new PostService(dbContext);
    }
    #endregion
}
