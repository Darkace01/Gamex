namespace Gamex.Service.Contract
{
    public interface IPostService
    {
        Task<bool> ArchivePost(Guid postId);
        Task<bool> CreatePost(PostCreateDTO postCreateDTO, ApplicationUser user);
        Task<bool> DeletePost(Guid postId, ApplicationUser user);
        IQueryable<PostDTO> GetAllPosts();
        Task<PostDTO> GetPost(Guid postId);
        Task<bool> UnArchivePost(Guid postId);
        Task<bool> UpdatePost(PostUpdateDTO postUpdateDTO, ApplicationUser user);
    }
}