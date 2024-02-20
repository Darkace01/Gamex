namespace Gamex.Service.Contract
{
    public interface ICommentService
    {
        Task<bool> CreateComment(CommentCreateDTO commentCreateDTO, string userId, CancellationToken cancellationToken = default);
        Task<bool> DeleteComment(Guid commentId, CancellationToken cancellationToken = default);
        Task<bool> DeleteCommentByPostId(Guid postId, CancellationToken cancellationToken = default);
        IQueryable<CommentDTO> GetAllCommentByPostId(Guid postId);
        IQueryable<CommentDTO> GetAllComments();
        Task<CommentDTO?> GetCommentById(Guid id, CancellationToken cancellationToken = default);
        Task<bool> UpdateComment(CommentUpdateDTO commentUpdateDTO, CancellationToken cancellationToken = default);
    }
}