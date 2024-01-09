namespace Gamex.Service.Implementation;

public class PostService : IPostService
{
    private readonly GamexDbContext _context;

    public PostService(GamexDbContext context)
    {
        _context = context;
    }

    public async Task<PostDTO> GetPost(Guid postId)
    {
        var post = await _context.Posts.AsNoTracking().FirstOrDefaultAsync(p => p.Id == postId);
        if (post == null)
        {
            return null;
        }

        return MapPostToDTO(post);
    }

    public IQueryable<PostDTO> GetAllPosts()
    {
        var posts = _context.Posts.AsNoTracking();
        return posts.Select(p => MapPostToDTO(p));
    }

    public async Task<bool> CreatePost(PostCreateDTO postCreateDTO, ApplicationUser user)
    {
        var post = new Post()
        {
            Title = postCreateDTO.Title,
            Content = postCreateDTO.Content,
            DateCreated = DateTime.UtcNow,
            DateModified = DateTime.UtcNow,
            UserId = user.Id,
            PictureId = postCreateDTO.PictureId

        };
        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdatePost(PostUpdateDTO postUpdateDTO, ApplicationUser user)
    {
        var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postUpdateDTO.Id && p.UserId == user.Id);
        if (post is not null)
        {
            post.Title = postUpdateDTO.Title;
            post.Content = postUpdateDTO.Content;
            post.DateModified = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<bool> DeletePost(Guid postId, ApplicationUser user)
    {
        var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId && p.UserId == user.Id);
        if (post == null)
        {
            return false;
        }
        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();
        return true;
    }

    private static PostDTO MapPostToDTO(Post post)
    {
        var comments = post.Comments?.Select(c => new CommentDTO(c.Id, c.Title, c.Content, c.IsArchived, new UserProfileDTO(c.User?.DisplayName, c.User?.Email, c?.User?.PhoneNumber, c?.User?.Picture?.FileUrl, c?.User?.Picture?.PublicId), c.PostId));

        return new PostDTO(post.Id, post.Title, post.Content, post.IsArchived, post?.Picture?.FileUrl, post?.Picture?.PublicId, new UserProfileDTO(post?.User?.DisplayName, post?.User?.Email, post?.User?.PhoneNumber, post?.User?.Picture?.FileUrl, post?.User?.Picture?.PublicId), comments);
    }
}
