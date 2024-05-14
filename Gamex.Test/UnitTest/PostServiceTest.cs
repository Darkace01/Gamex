namespace Gamex.Test.UnitTest;

public class PostServiceTest : TestBase
{
    [Fact]
    public async Task GetPost_ShouldReturnPost()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetPost_ShouldReturnPost));
        var postService = MockPostService(dbContext);
        var postToGet = dbContext.Posts.First();

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
        var saved = await postService.CreatePostMock(newPost, testUser);

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
        var updated = await postService.UpdatePostMock(updatedPost, testUser);

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
        var deleted = await postService.DeletePost(postToDelete.Id, testUser);

        // Assert
        var deletedPostInDb = dbContext.Posts.FirstOrDefault(p => p.Id == postToDelete.Id);
        Assert.Null(deletedPostInDb);
        Assert.True(deleted);
    }

    [Fact]
    public void GetAllPosts_ShouldReturnAllPosts()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetAllPosts_ShouldReturnAllPosts));
        var postService = MockPostService(dbContext);

        // Act
        var posts = postService.GetAllPosts();

        // Assert
        Assert.NotNull(posts);
        Assert.Equal(2, posts.Count());
    }

    [Fact]
    public async Task ArchivePost_ShouldArchivePost()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(ArchivePost_ShouldArchivePost));
        var postService = MockPostService(dbContext);
        var postToArchive = dbContext.Posts.FirstOrDefault();

        // Act
        var archived = await postService.ArchivePost(postToArchive.Id);

        // Assert
        var archivedPostInDb = dbContext.Posts.FirstOrDefault(p => p.Id == postToArchive.Id);
        Assert.NotNull(archivedPostInDb);
        Assert.True(archived);
        Assert.True(archivedPostInDb.IsArchived);
    }

    [Fact]
    public async Task UnArchivePost_ShouldUnArchivePost()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(UnArchivePost_ShouldUnArchivePost));
        var postService = MockPostService(dbContext);
        var postToUnArchive = dbContext.Posts.FirstOrDefault();

        // Act
        var unArchived = await postService.UnArchivePost(postToUnArchive.Id);

        // Assert
        var unArchivedPostInDb = dbContext.Posts.FirstOrDefault(p => p.Id == postToUnArchive.Id);
        Assert.NotNull(unArchivedPostInDb);
        Assert.True(unArchived);
        Assert.False(unArchivedPostInDb.IsArchived);
    }
    [Fact]
    public async Task CreatePostWithTags_ShouldCreatePostWithTags()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(CreatePostWithTags_ShouldCreatePostWithTags));
        var postService = MockPostService(dbContext);
        var testUser = dbContext.Users.FirstOrDefault();
        var tags = dbContext.Tags.Take(2).ToList();
        PostCreateDTO newPost = new()
        {
            Title = "Test Post",
            Content = "Test Post Content",
            IsArchived = false,
            PictureId = null,
            Picture = null,
            UserId = testUser.Id,
            TagIds = tags.Select(t => t.Id).ToList()
        };

        // Act
        var saved = await postService.CreatePostMock(newPost, testUser);

        // Assert
        Assert.True(saved);
        var savedPost = dbContext.Posts.Include(p => p.PostTags).FirstOrDefault(p => p.Title == newPost.Title);
        Assert.NotNull(savedPost);
        Assert.Equal(tags.Count, savedPost.PostTags.Count);
    }
    #region Helpers
    private PostService MockPostService(GamexDbContext dbContext)
    {
        return new PostService(dbContext);
    }
    #endregion
}
