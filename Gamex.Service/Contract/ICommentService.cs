namespace Gamex.Service.Contract
{
    /// <summary>
    /// Represents the interface for managing comments.
    /// </summary>
    public interface ICommentService
    {
        /// <summary>
        /// Creates a new comment.
        /// </summary>
        /// <param name="commentCreateDTO">The DTO containing the comment details.</param>
        /// <param name="userId">The ID of the user creating the comment.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A boolean indicating whether the comment was created successfully.</returns>
        Task<bool> CreateComment(CommentCreateDTO commentCreateDTO, string userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a comment by its ID.
        /// </summary>
        /// <param name="commentId">The ID of the comment to delete.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A boolean indicating whether the comment was deleted successfully.</returns>
        Task<bool> DeleteComment(Guid commentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes all comments associated with a post.
        /// </summary>
        /// <param name="postId">The ID of the post.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A boolean indicating whether the comments were deleted successfully.</returns>
        Task<bool> DeleteCommentByPostId(Guid postId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all comments associated with a post, paginated.
        /// </summary>
        /// <param name="postId">The ID of the post.</param>
        /// <param name="take">The number of comments to take per page.</param>
        /// <param name="skip">The number of comments to skip.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A pagination object containing the comments.</returns>
        Task<PaginationDTO<CommentDTO>> GetAllCommentByPostId(Guid postId, int take = 10, int skip = 0, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all comments, paginated.
        /// </summary>
        /// <param name="take">The number of comments to take per page.</param>
        /// <param name="skip">The number of comments to skip.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A pagination object containing the comments.</returns>
        Task<PaginationDTO<CommentDTO>> GetAllComments(int take = 10, int skip = 0, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a comment by its ID.
        /// </summary>
        /// <param name="id">The ID of the comment.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The <see cref="CommentDTO"/> representing the comment, or null if not found.</returns>
        Task<CommentDTO?> GetCommentById(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates a comment.
        /// </summary>
        /// <param name="commentUpdateDTO">The DTO containing the updated comment details.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A boolean indicating whether the comment was updated successfully.</returns>
        Task<bool> UpdateComment(CommentUpdateDTO commentUpdateDTO, CancellationToken cancellationToken = default);
    }
}