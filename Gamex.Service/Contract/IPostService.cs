
namespace Gamex.Service.Contract
{
    public interface IPostService
    {
        /// <summary>
        /// Archives a post.
        /// </summary>
        /// <param name="postId">The ID of the post to archive.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a boolean value indicating whether the post was successfully archived.</returns>
        Task<bool> ArchivePost(Guid postId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new post.
        /// </summary>
        /// <param name="postCreateDTO">The DTO containing the data for creating the post.</param>
        /// <param name="user">The user creating the post.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a boolean value indicating whether the post was successfully created.</returns>
        Task<bool> CreatePost(PostCreateDTO postCreateDTO, ApplicationUser user, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new post mock.
        /// </summary>
        /// <param name="postCreateDTO">The DTO containing the data for creating the post.</param>
        /// <param name="user">The user creating the post.</param>
        /// <returns>A boolean value indicating whether the post mock was successfully created.</returns>
        Task<bool> CreatePostMock(PostCreateDTO postCreateDTO, ApplicationUser user);

        /// <summary>
        /// Deletes a post.
        /// </summary>
        /// <param name="postId">The ID of the post to delete.</param>
        /// <param name="user">The user deleting the post.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a boolean value indicating whether the post was successfully deleted.</returns>
        Task<bool> DeletePost(Guid postId, ApplicationUser user, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all posts.
        /// </summary>
        /// <returns>An IQueryable of PostDTO representing all posts.</returns>
        IQueryable<PostDTO> GetAllPosts();

        /// <summary>
        /// Gets all posts with optional filtering and pagination.
        /// </summary>
        /// <param name="TagIds">The IDs of the tags to filter by.</param>
        /// <param name="take">The maximum number of posts to take.</param>
        /// <param name="skip">The number of posts to skip.</param>
        /// <param name="s">The search string to filter by.</param>
        /// <returns>An IQueryable of PostDTO representing the filtered and paginated posts.</returns>
        IQueryable<PostDTO> GetAllPosts(IEnumerable<string> TagIds, int take = 10, int skip = 0, string s = "");

        /// <summary>
        /// Gets a post by ID.
        /// </summary>
        /// <param name="postId">The ID of the post to get.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the PostDTO if found, or null if not found.</returns>
        Task<PostDTO?> GetPost(Guid postId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Unarchives a post.
        /// </summary>
        /// <param name="postId">The ID of the post to unarchive.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a boolean value indicating whether the post was successfully unarchived.</returns>
        Task<bool> UnArchivePost(Guid postId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates a post.
        /// </summary>
        /// <param name="postUpdateDTO">The DTO containing the data for updating the post.</param>
        /// <param name="user">The user updating the post.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a boolean value indicating whether the post was successfully updated.</returns>
        Task<bool> UpdatePost(PostUpdateDTO postUpdateDTO, ApplicationUser user, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates a post mock.
        /// </summary>
        /// <param name="postUpdateDTO">The DTO containing the data for updating the post.</param>
        /// <param name="user">The user updating the post.</param>
        /// <returns>A boolean value indicating whether the post mock was successfully updated.</returns>
        Task<bool> UpdatePostMock(PostUpdateDTO postUpdateDTO, ApplicationUser user);
    }
}