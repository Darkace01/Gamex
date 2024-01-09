namespace Gamex.Test.UnitTest;

public class CommentServiceTest:TestBase
{

    [Fact]
    public async Task GetComment_ShouldReturnComment()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetComment_ShouldReturnComment));
        var commentService = MockCommentService(dbContext);
        var commentToGet = dbContext.Comments.FirstOrDefault();

        // Act
        var comment = await commentService.GetCommentById(commentToGet.Id);

        // Assert
        Assert.NotNull(comment);
        Assert.Equal(commentToGet.Id, comment.Id);
    }

    [Fact]
    public async Task CreateComment_ShouldCreateComment()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(CreateComment_ShouldCreateComment));
        var commentService = MockCommentService(dbContext);
        var postToGet = dbContext.Posts.FirstOrDefault();
        var testUser = dbContext.Users.FirstOrDefault();

        CommentCreateDTO newComment = new("Test Comment", "Test Content 3", false, postToGet.Id);

        // Act
        var saved = await commentService.CreateComment(newComment,testUser.Id);

        // Assert
        Assert.True(saved);
        // Add more assertions to check the properties of the created comment
    }

    [Fact]
    public async Task UpdateComment_ShouldUpdateComment()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(UpdateComment_ShouldUpdateComment));
        var commentService = MockCommentService(dbContext);
        var commentToUpdate = dbContext.Comments.FirstOrDefault();

        CommentUpdateDTO updatedComment = new(commentToUpdate.Id, "Test Comment", "Test Content 3", false, commentToUpdate.PostId);

        // Act
        await commentService.UpdateComment(updatedComment);

        // Assert
        var updatedCommentInDb = dbContext.Comments.FirstOrDefault(p => p.Id == commentToUpdate.Id);
        Assert.NotNull(updatedCommentInDb);
        // Add more assertions to check the properties of the updated comment
    }

    [Fact]
    public async Task DeleteComment_ShouldDeleteComment()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(DeleteComment_ShouldDeleteComment));
        var commentService = MockCommentService(dbContext);
        var commentToDelete = dbContext.Comments.FirstOrDefault();

        // Act
        var deleted = await commentService.DeleteComment(commentToDelete.Id);

        // Assert
        Assert.True(deleted);
        var deletedCommentInDb = dbContext.Comments.FirstOrDefault(p => p.Id == commentToDelete.Id);
        Assert.Null(deletedCommentInDb);
    }

    [Fact]
    public void GetAllComments_ShouldReturnComments()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetAllComments_ShouldReturnComments));
        var commentService = MockCommentService(dbContext);

        // Act
        var comments = commentService.GetAllComments().ToList();

        // Assert
        Assert.NotNull(comments);
        Assert.Equal(2, comments.Count());
    }

    [Fact]
    public void GetCommentsByPostId_ShouldReturnComments()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetCommentsByPostId_ShouldReturnComments));
        var commentService = MockCommentService(dbContext);
        var postToGet = dbContext.Posts.AsNoTracking().FirstOrDefault(x => x.Comments.Count() > 0);

        // Act
        var comments = commentService.GetAllCommentByPostId(postToGet.Id);

        // Assert
        Assert.NotNull(comments);
        Assert.Equal(2, comments.Count());
    }

    #region Helpers
    private CommentService MockCommentService(GamexDbContext dbContext)
    {
        return new CommentService(dbContext);
    }
    #endregion
}
