
namespace Gamex.Service.Contract
{
    public interface IPostService
    {
        Task<bool> ArchivePost(Guid postId, CancellationToken cancellationToken = default);
        Task<bool> CreatePost(PostCreateDTO postCreateDTO, ApplicationUser user, CancellationToken cancellationToken = default);
        Task<bool> CreatePostMock(PostCreateDTO postCreateDTO, ApplicationUser user);
        Task<bool> DeletePost(Guid postId, ApplicationUser user, CancellationToken cancellationToken = default);
        IQueryable<PostDTO> GetAllPosts();
        IQueryable<PostDTO> GetAllPosts(IEnumerable<string> TagIds, int take = 10, int skip = 0, string s = "");
        Task<PostDTO?> GetPost(Guid postId, CancellationToken cancellationToken = default);
        Task<bool> UnArchivePost(Guid postId, CancellationToken cancellationToken = default);
        Task<bool> UpdatePost(PostUpdateDTO postUpdateDTO, ApplicationUser user, CancellationToken cancellationToken = default);
        Task<bool> UpdatePostMock(PostUpdateDTO postUpdateDTO, ApplicationUser user);
    }
}