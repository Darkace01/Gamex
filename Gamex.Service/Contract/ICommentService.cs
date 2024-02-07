namespace Gamex.Service.Contract
{
    public interface ICommentService
    {
        Task<bool> CreateComment(CommentCreateDTO commentCreateDTO, string userId);
        Task<bool> DeleteComment(Guid commentId);
        Task<bool> DeleteCommentByPostId(Guid postId);
        IQueryable<CommentDTO> GetAllCommentByPostId(Guid postId);
        IQueryable<CommentDTO> GetAllComments();
        CommentDTO GetCommentById(Guid id);
        Task<bool> UpdateComment(CommentUpdateDTO commentUpdateDTO);
    }
}